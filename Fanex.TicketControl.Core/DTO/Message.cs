// ----------------------------------------------------------------------
// <copyright file="Message.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2013/08/29">StyleCop</change>
//   <change who="" date=""></change>
// </history>
namespace Fanex.TicketControl.Core.DTO
{
    /// <summary>
    /// Message class.
    /// </summary>
    public class Message
    {
        private string _content_ch = string.Empty;
        private string _content_cs = string.Empty;
        private string _content_en = string.Empty;
        private string _content_es = string.Empty;
        private string _content_it = string.Empty;
        private string _content_jp = string.Empty;
        private string _content_ko = string.Empty;
        private string _content_th = string.Empty;
        private string _creatorName = string.Empty;

        public string Content_ch
        {
            get
            {
                return _content_ch;
            }

            set
            {
                _content_ch = value;
            }
        }

        public string Content_cs
        {
            get
            {
                return _content_cs;
            }

            set
            {
                _content_cs = value;
            }
        }

        public string Content_en
        {
            get
            {
                return _content_en;
            }

            set
            {
                _content_en = value;
            }
        }

        public string Content_es
        {
            get
            {
                return _content_es;
            }

            set
            {
                _content_es = value;
            }
        }

        public string Content_it
        {
            get
            {
                return _content_it;
            }

            set
            {
                _content_it = value;
            }
        }

        public string Content_jp
        {
            get
            {
                return _content_jp;
            }

            set
            {
                _content_jp = value;
            }
        }

        public string Content_ko
        {
            get
            {
                return _content_ko;
            }

            set
            {
                _content_ko = value;
            }
        }

        public string Content_th
        {
            get
            {
                return _content_th;
            }

            set
            {
                _content_th = value;
            }
        }

        public int CreatorId
        {
            get;
            set;
        }

        public string CreatorName
        {
            get
            {
                return _creatorName;
            }

            set
            {
                _creatorName = value;
            }
        }

        public bool IsSticky
        {
            get;
            set;
        }

        public int MessageId
        {
            get;
            set;
        }

        public int MessageType
        {
            get;
            set;
        }

        public int MsgType
        {
            get;
            set;
        }
    }
}