using System.Collections.Generic;

namespace Unity_Framework.Scripts.Import.Interface.Manager
{
    public interface IHandler<TID, TItem> :IIsValid where TItem : IHandlerItem<TID>
    {
        #region f/p
    
        Dictionary<TID, TItem> Handles { get; }
        #endregion
        #region custom methods
        void Add(TItem _item);
        void Remove(TItem _item);
        bool IsExist(TItem _item);
        bool IsExist(TID _id);

        #endregion
    }
}