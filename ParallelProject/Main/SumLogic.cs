using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParallelProject;

namespace ParallelProject.Main
{
    public class SumLogic // OBJETO DE INSTANCIA REPARTIDA PARA LOS HILOS DE EJECUCIÓN
    {
        public int index { get; set; }
        public int result { get; set; }
        public int limSup { get; set; }
        public int limInf { get; set; }

        public SumLogic(int index, int result, int limInf, int limSup) {
            this.index = index;
            this.result = result;
            this.limInf = limInf;
            this.limSup = limSup;
        }
    }
}
