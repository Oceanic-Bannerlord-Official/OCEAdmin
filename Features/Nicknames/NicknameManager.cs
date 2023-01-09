using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCEAdmin.Features.Nicknames
{
    public class NicknameManager
    {
        private static NicknameManager instance;
        public static NicknameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NicknameManager();
                }
                return instance;
            }
        }

        public NicknameManager()
        {
            Storage = new List<Tuple<string, string>>();
        }

        public List<Tuple<string, string>> Storage;

        public void AddToStorage(string steam, string nickname)
        {
            var found = false;
            for (int i = 0; i < Storage.Count; i++)
            {
                if (Storage[i].Item1 == steam)
                {
                    Storage[i] = Tuple.Create(steam, nickname);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Storage.Add(Tuple.Create(steam, nickname));
            }
        }

        public string GetNicknameFromStorage(string steam)
        {
            foreach (var pair in Storage)
            {
                if (pair.Item1 == steam)
                {
                    return pair.Item2;
                }
            }

            return null;
        }
    }
}
