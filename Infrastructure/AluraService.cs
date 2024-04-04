using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AluraService : SeleniumService
    {   
        public AluraService() {
            this.GetSelenium("https://www.alura.com.br/");
        }
    }
}
