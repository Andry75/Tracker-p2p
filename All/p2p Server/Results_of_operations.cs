using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace p2p_Server
{
    enum Results_of_operations
    {
        Rgistration_Feedback_OK=0,
        Rgistration_Feedback_Server_Error = 2,
        Rgistration_Feedback_Client_not_found=1,
        Receive_Clients_matirial_not_found =0,
        Update_Info_Feedback_OK = 0,
        Update_Info_Feedback_Bad_informatio = 1,
        Update_Info_Feedback_Server_Error = 2


    }
}
