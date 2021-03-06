﻿using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace CommuteTimingWebsite
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();
            Formatting=Formatting.Indented;
        }

        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }


        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }


        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)

                throw new ArgumentNullException("context");


            HttpResponseBase response = context.HttpContext.Response;


            response.ContentType = !string.IsNullOrEmpty(ContentType)
                                       ? ContentType
                                       : "application/json";


            if (ContentEncoding != null)

                response.ContentEncoding = ContentEncoding;


            if (Data != null)
            {
                var writer = new JsonTextWriter(response.Output) {Formatting = Formatting};
                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}