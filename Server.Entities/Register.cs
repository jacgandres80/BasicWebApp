using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Server.Models
{
    [Serializable]
    [DataContract(Name = "Register")]
    public class Register
    {
        [DataMember]
        [Key]
        public int IdRegister { get; set; }

        [DataMember]
        public DateTime DateRegister { get; set; }

        [DataMember]
        public int SumServerPort { get; set; }

        [DataMember]
        public int ServerWebAppPort { get; set; }

        [DataMember]
        public string SumServerAddress { get; set; }

        [DataMember]
        public string ServerAddress { get; set; }

        [DataMember]
        public REGISTER_STATE State { get; set; }

        [DataMember]
        public string SumServerName { get; set; }

        [DataMember]
        public string HostName { get; set; }

        [DataMember]
        public string HostUrl { get; set; }

        [DataMember]
        public ACTION_REGISTER ActionRegister { get; set; }

        [DataMember]
        public ACTION_SUM_SERVER ActionSumServer { get; set; }
    }

    [Serializable]
    public enum ACTION_REGISTER : int
    {
        Register = 1,
        Unregister = 2
    }

    [Serializable]
    public enum REGISTER_STATE : int
    {
        Conected = 1,
        Close = 2,
        Pending = 3,
        Error = 4
    }

    public enum ACTION_SUM_SERVER : int
    {
        Conected = 1,
        Disconected = 2
    }
}