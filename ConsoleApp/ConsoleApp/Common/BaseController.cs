using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Models;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp.Models.Constant;

namespace ConsoleApp.Common
{
    public class BaseController : Controller
    {
        static ConnectionFactory _factory = new ConnectionFactory { HostName = "localhost" };
        static IConnection _connection;
        public static IConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    Connect(5);
                }
                return _connection;
            }
        }
        static IModel _channel;
        string _replyTo { get; set; }
        IBasicProperties _basicProps { get; set; }

        static void MsgReceived(object model, BasicDeliverEventArgs ea)
        {
            try
            {
                var response = new DataContext();

                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                var message = Encoding.UTF8.GetString(body);
                var context = Newtonsoft.Json.Linq.JObject
                .Parse(message)
                .ToObject<DataContext>();

                var c = Engine.GetController<BaseController>(context.ControllerName);
                c._replyTo = props.ReplyTo;
                c._basicProps = replyProps;

                if (c != null)
                {
                    if (context.ActionName == null)
                    {
                        context.ActionName = "Default";
                    }
                    var action = c.GetMethod(context.ActionName);
                    if (action != null)
                    {
                        var v = context.Value ?? new object { };
                        AsyncEngine.CreateThread(() => action.Invoke(c, new object[] { v }));
                    }
                }
            }
            catch (Exception ex)
            {
                Screen.Error(ex.Message);
            }
            finally
            {
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
        }

        static void Connect(int checkConnectionSeconds = 0)
        {
            if (_connection != null && _connection.IsOpen) return;
            _connection.ConnectionShutdown += (s, e) =>
            {
                Screen.Warning("Connection closed");
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "rpc_queue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            var consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: "rpc_queue",
                                 autoAck: false,
                                 consumer: consumer);
            Console.WriteLine(" [x] Awaiting RPC requests");

            consumer.Received += MsgReceived;

            if (checkConnectionSeconds > 0)
            {
                int interval = checkConnectionSeconds * 1000;
                AsyncEngine.CreateThread(() =>
                {
                    while (true)
                    {
                        System.Threading.Thread.Sleep(interval);
                        Connect();
                    }
                });
            }
        }
        
        static byte[] GetEncodeBytes(object v)
        {
            var content = Newtonsoft.Json.Linq.JObject.FromObject(v).ToString();
            return System.Text.Encoding.UTF8.GetBytes(content);
        }

        protected void Publish(object value)
        {
            var data = new DataContext();
            data.Code = Enums.ResponseStatus.Succucess.ToString();
            data.Value = value;

            var bytes = GetEncodeBytes(value);
            _channel.BasicPublish(exchange: string.Empty,
                                     routingKey: this._replyTo,
                                     basicProperties: this._basicProps,
                                     body: bytes);
        }

        protected void Error(string msg)
        {
            var data = new DataContext();
            data.Code = Enums.ResponseStatus.Error.ToString();
            data.Message = msg;

            var bytes = GetEncodeBytes(data);
            _channel.BasicPublish(exchange: string.Empty,
                                     routingKey: this._replyTo,
                                     basicProperties: this._basicProps,
                                     body: bytes);
        }
    }
}
