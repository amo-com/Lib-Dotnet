using Amo.Lib.Attributes;

namespace Amo.Lib
{
    [Autowired]
    public interface IShowEvent
    {
        void ShowButton(bool isShow);
        void ShowMessage(string msg)
#if NETSTANDARD2_0
            ;
#elif NETSTANDARD2_1
        {
            ShowMessage(msg, (int)Enums.LogLevel.Info);
        }
#endif
        void ShowMessage(string msg, int level);
        void ShowProgress(int index, int count);
    }
}
