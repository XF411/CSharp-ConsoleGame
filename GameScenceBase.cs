using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Learn
{
    public abstract class GameScenceBase
    {
        public E_ScenceType e_ScenceType;

        public abstract void Update();

        public abstract void OnExit();
    }
}
