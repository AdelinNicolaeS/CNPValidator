using System;
using System.Collections.Generic;
using System.Text;

namespace CNPValidator
{
    interface IChecker
    {
        bool CheckLength();
        bool CheckFirstDigit();
        bool Check2and3();
        bool Check4and5();
        bool Check6and7();
        bool Check8and9();
        bool Check10and11and12();
        bool Check13();


    }
}
