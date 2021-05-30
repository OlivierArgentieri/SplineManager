namespace Unity_Framework.Scripts.Import.Interface.Manager
{
    public interface IHandlerItem<TID> : IIsValid
    {
        #region f/p
        TID ID { get; }
        #endregion
    
        #region custom methods
        void Register();
        void Unregister();
        #endregion
    }
}