using System;
using System.Reflection;
using BLL.Helpers.Attributes;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;

namespace PaymentAPI.Helpers.Attributes
{
    public class LoggingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (var item in filterContext.ActionArguments)
            {
                var type = item.Value.GetType();
                dynamic obj = Convert.ChangeType(item.Value, type);
                var props = type.GetProperties();

                if (props.Length == 0)
                {
                    LogContext.PushProperty(item.Key, item.Value);
                }
                else
                {
                    foreach (var prop in props)
                    {
                        bool logOff = LogOffCheck(prop);

                        if (!logOff)
                        {
                            dynamic value = prop.GetValue(obj, null);
                            var name = prop.Name;
                            LogContext.PushProperty(name, value); 
                        }
                    }
                };
            }
                
            Log.Information($"Request information");
        }

        private bool LogOffCheck(PropertyInfo prop)
        {
            bool logOff = false;

            foreach (var attribute in prop.CustomAttributes)
            {
                if (attribute.AttributeType.Name.Equals(typeof(LogOffAttribute).Name))
                    logOff = true;
            }

            return logOff;
        }
    }
}