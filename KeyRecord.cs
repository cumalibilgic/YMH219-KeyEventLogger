using System;

namespace KeyEventLogger
{
    /// <summary>Tek bir klavye olayının kaydı.</summary>
    public class KeyRecord
    {
        public const string KeyDownType = "KeyDown";
        public const string KeyUpType = "KeyUp";

        public DateTime Time { get; }
        public string KeyName { get; }
        public string EventType { get; } // KeyDownType veya KeyUpType

        public KeyRecord(string keyName, string eventType)
        {
            Time = DateTime.Now;
            KeyName = keyName;
            EventType = eventType;
        }

        public override string ToString()
        {
            return $"[{Time:yyyy-MM-dd HH:mm:ss.fff}] {EventType,-7} {KeyName}";
        }
    }
}
