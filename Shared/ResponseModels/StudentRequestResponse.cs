using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ResponseModels
{
    public class StudentRequestResponse
    {
        public int ID { get; set; }
        public bool IsSuccess { get; set; }
        public decimal? Amount { get; set; }

        public string Message { get; set; }

        public bool isCV { get; set; }
        public bool isTranscript { get; set; }
        public bool isId { get; set; }
    }
}
