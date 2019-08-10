// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectStage.cs" company="">
//   
// </copyright>
// <summary>
//   The connect stage.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


    using Regulus.Remote;
    using Regulus.Utility;

    /// <summary>
    /// The connect stage.
    /// </summary>
    public class ConnectStage : IStage
    {
        /// <summary>
        /// The _ ip.
        /// </summary>
        private string _Ip;

        /// <summary>
        /// The _ port.
        /// </summary>
        private int _Port;

        /// <summary>
        /// The _ provider.
        /// </summary>
        private INotifier<IConnect> _Provider;

        /// <summary>
        /// The done callback.
        /// </summary>
        public delegate void DoneCallback();

        /// <summary>
        /// The success event.
        /// </summary>
        public event DoneCallback SuccessEvent;

        /// <summary>
        /// The fail event.
        /// </summary>
        public event DoneCallback FailEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectStage"/> class.
        /// </summary>
        /// <param name="_Ip">
        /// The _ ip.
        /// </param>
        /// <param name="_Port">
        /// The _ port.
        /// </param>
        /// <param name="providerNotice">
        /// The provider notice.
        /// </param>
        public ConnectStage(string _Ip, int _Port, INotifier<IConnect> providerNotice)
        {
            
            this._Ip = _Ip;
            this._Port = _Port;
            this._Provider = providerNotice;
        }

        /// <summary>
        /// The leave.
        /// </summary>
        void IStage.Leave()
        {
            _Provider.Supply -= _Connect;
        }

        /// <summary>
        /// The enter.
        /// </summary>
        void IStage.Enter()
        {
            _Provider.Supply += _Connect;            
        }

        /// <summary>
        /// The _ connect.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        private void _Connect(IConnect obj)
        {
            _Provider.Supply -= _Connect;
        
            obj.Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(_Ip) , _Port)).OnValue += _Result;
        }

        /// <summary>
        /// The _ result.
        /// </summary>
        /// <param name="success">
        /// The success.
        /// </param>
        private void _Result(bool success)
        {

            if (success)
                SuccessEvent();
            else
            {
                FailEvent();
            }
                

            
        }

        /// <summary>
        /// The update.
        /// </summary>
        void IStage.Update()
        {
            
        }
    }

