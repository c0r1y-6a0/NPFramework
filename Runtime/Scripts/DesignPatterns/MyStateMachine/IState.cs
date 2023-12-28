namespace NP
{
    public interface IState
    {
        void OnEnter();
        void OnLeave();
    }
}