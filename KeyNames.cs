using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyEventLogger
{
    /// <summary>
    /// Tuş kodlarını okunabilir adlara çevirir. Oem tuşları (Oem1, Oem3 vb.) için
    /// aktif klavye düzeninin gerçek karakterini kullanır; böylece Türkçe Q düzeninde
    /// Ş, İ, Ğ, Ü, Ö, Ç gibi harfler "Oem1" yerine kendi adlarıyla görünür.
    /// </summary>
    public static class KeyNames
    {
        private const uint MAPVK_VK_TO_CHAR = 2;

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint code, uint mapType);

        public static string GetName(Keys key)
        {
            // Üst sıradaki rakam tuşları "D1" yerine "1" olarak gösterilsin
            if (key >= Keys.D0 && key <= Keys.D9)
                return ((char)('0' + (key - Keys.D0))).ToString();

            if (IsOemKey(key))
            {
                uint result = MapVirtualKey((uint)key, MAPVK_VK_TO_CHAR);
                char c = (char)(result & 0xFFFF);
                if (c != '\0' && !char.IsControl(c))
                    return c.ToString();
            }

            return key.ToString();
        }

        private static bool IsOemKey(Keys key)
        {
            int code = (int)key;
            return (code >= 186 && code <= 192) || (code >= 219 && code <= 226);
        }
    }
}
