using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


  public  interface iIntermediaries
        {
    void Init();
    void Updata();
    void FixUpdata();

    void GroupMessage();
    void ReceiveMessage(MessageInfo info);
    }

