using System;
using System.Collections.Generic;

namespace AddQualADTCommandEventFunctionApp.Model.EventGrid
{
    public class DataModel
    {
        public string ModelId { get; set; }
        public List<object> Patch { get; set; }
    }
}