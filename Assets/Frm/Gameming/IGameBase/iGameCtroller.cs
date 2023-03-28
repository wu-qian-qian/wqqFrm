using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

  public interface  iGameCtroller        {

      void Start();
     // void Init();
      void Change(string interName,params Object[] obj);
      void Updata();
      void FixUpdata();
      void Init();
      void SetState();
}
