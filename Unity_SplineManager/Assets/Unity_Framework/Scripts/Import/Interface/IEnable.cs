namespace Unity_Framework.Scripts.Import.Interface
{
    public interface IEnable
    {
        #region f/p

        bool IsEnable { get; }

        #endregion

        #region custom methods

        void SetEnable(bool _value);

        #endregion
    }
}