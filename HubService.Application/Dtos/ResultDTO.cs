using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubService.Application.Dtos
{
    public class ResultDTO
    {
        public bool Success = true;
        public int Code = 200;
        public string ErrorMessage { get; set; }
        public Object Data { get; set; }
    }
}
