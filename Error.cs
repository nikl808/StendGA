using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

//modify to thread safe singleton

namespace stend
{
    class MsgListEvent : EventArgs
    {
        //getting the constructed message string
        public string MsgLine { get; internal set; }

        //build message string
        public void AddToList(string caption, string text)
        {
            StringBuilder str = new StringBuilder();
            str.Append(DateTime.Now);
            str.Append(". ");
            str.Append(text);
            str.Append(" ");
            str.Append(caption);
            str.Append(" ");
            MsgLine = str.ToString();
        }
    }

    class Error
    {
        //Shared event
        public event EventHandler<MsgListEvent> _list;
        
        //full lazy implementation
        public static Error instance { get { return ErrorHolder.instance; } }

        private class ErrorHolder { internal static readonly Error instance = new Error();}
        //...

        public void HandleWarningMessage(string text ) { MessageBox.Show(text, "Warning" ); }
        //Show MessageBox with exception message
        public void HandleExceptionMessage(Exception ex) { MessageBox.Show(ex.Message,"Exception: "); }

        //Show MessageBox with Error
        public void HandleErrorMessage(string error) { MessageBox.Show(error, "Error: "); }
        
        //Create new string message with custom error
        public void HandleErrorLog(string caption, string text){ AddNewLine(text,caption); }
                
        //Create new string message with system/modbus errors
        public void HandleExceptionLog(Exception ex)
        {
            //Connection exception
            //No response from server.
            //The server maybe close the connection, or response timeout.
            string msg = ex.GetType().ToString();
            msg = msg.Remove(msg.IndexOf("."), msg.Length - (msg.IndexOf(".")));
           
            if (msg == "Modbus")//else if(exception.Source.Equals("nModbusCE"))
            {
                string str = ex.Message;
                int FunctionCode;
                string ExceptionCode;

                str = str.Remove(0, str.IndexOf("\r\n") + 17);
                FunctionCode = Convert.ToInt16(str.Remove(str.IndexOf("\r\n"), str.Length - str.IndexOf("\r\n")));
                AddNewLine("ERROR","Function Code: " + FunctionCode.ToString("X"));
                
                str = str.Remove(0, str.IndexOf("\r\n") + 17);
                ExceptionCode = str.Remove(str.IndexOf("-"), str.Length - str.IndexOf("-"));
                switch (ExceptionCode.Trim())
                {
                    case "1":
                        AddNewLine("ERROR","Exception Code: " + ExceptionCode.Trim() + "----> Illegal function!");
                        break;
                    case "2":
                        AddNewLine("ERROR", "Exception Code: " + ExceptionCode.Trim() + "----> Illegal data address!");
                        break;
                    case "3":
                        AddNewLine("ERROR", "Exception Code: " + ExceptionCode.Trim() + "----> Illegal data value!");
                        break;
                    case "4":
                        AddNewLine("ERROR", "Exception Code: " + ExceptionCode.Trim() + "----> Slave device failure!");
                        break;
                }
            }
            else AddNewLine("ERROR", ex.Message);
        }

        //Service function
        private void AddNewLine(string caption, string text)
        {
            MsgListEvent line = new MsgListEvent();
            line.AddToList(caption, text);
            onNewLineReached(line);
        }

        private void onNewLineReached(MsgListEvent ev)
        {
            var handler = _list;
            if (handler != null) { handler(this, ev); }
        }
        //...

    }

    class QuestionMessage
    {
        //Show asking messageBox and return choice
        public bool Message(string text, string caption)
        {
            DialogResult result;
            result = MessageBox.Show(caption, text, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes) return true;
            else return false;
        }
    }
}

/*
//Modbus exception codes definition
                            
* Code   * Name                                      * Meaning
01       ILLEGAL FUNCTION                            The function code received in the query is not an allowable action for the server.
                         
02       ILLEGAL DATA ADDRESS                        The data addrdss received in the query is not an allowable address for the server.
                         
03       ILLEGAL DATA VALUE                          A value contained in the query data field is not an allowable value for the server.
                           
04       SLAVE DEVICE FAILURE                        An unrecoverable error occurred while the server attempting to perform the requested action.
                             
05       ACKNOWLEDGE                                 This response is returned to prevent a timeout error from occurring in the client (or master)
                                                     when the server (or slave) needs a long duration of time to process accepted request.                          

06       SLAVE DEVICE BUSY                           The server (or slave) is engaged in processing a longЎVduration program command , and the 
                                                     client (or master) should retransmit the message later when the server (or slave) is free.
                             
08       MEMORY PARITY ERROR                         The server (or slave) attempted to read record file, but detected a parity error in the memory.
                             
0A       GATEWAY PATH UNAVAILABLE                    The gateway is misconfigured or overloaded.
                             
0B       GATEWAY TARGET DEVICE FAILED TO RESPOND     No response was obtained from the target device. Usually means that the device is not present on the network.

*/