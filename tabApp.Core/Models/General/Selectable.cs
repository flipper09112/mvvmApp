using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models.General
{
    public class Selectable<T> 
    {
        public T Data { get; }
        public bool Selected { get; set; }

        public Selectable(T data) 
        {
            Data = data;
            Selected = true;
        }
    }
}
