using System;

using Regulus.Utility;

namespace Regulus.Remote
{


    /// <summary>
    ///     �N�z��
    /// </summary>
    /// 

    public interface IAgent : IUpdatable
    {
        /// <summary>
        ///     �P���ݵo���_�u
        ///     �I�sDisconnect���|�o�ͦ��ƥ�
        /// </summary>
        event Action BreakEvent;

        /// <summary>
        ///     �s�u���\�ƥ�
        /// </summary>
        event Action ConnectEvent;

        /// <summary>
        ///     Ping
        /// </summary>
        long Ping { get; }

        /// <summary>
        ///     �O�_���s�u���A
        /// </summary>
        bool Connected { get; }

        /// <summary>
        ///     �d�ߤ�������q����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        INotifier<T> QueryNotifier<T>();

        /// <summary>
        ///     �s�u
        /// </summary>        
        /// <returns>�p�G�s�u���\�|�o��OnValue�Ǧ^true</returns>
        Value<bool> Connect(System.Net.IPEndPoint ip);

        /// <summary>
        ///     �_�u
        /// </summary>
        void Disconnect();

        /// <summary>
        /// ���~����k�I�s
        /// �p�G�I�s����k�ѼƦ��~�h�|�^�Ǧ��T��.
        /// �ƥ�Ѽ�:
        ///     1.��k�W��
        ///     2.���~�T��
        /// �|�o�ͦ��T���q�`�O�]��client�Pserver�������ۮe�ҭP.
        /// </summary>
        event Action<string , string> ErrorMethodEvent;


        /// <summary>
        /// ���ҿ��~
        /// �N��P���A���ݪ����ҽX����
        /// �ƥ�Ѽ�:
        ///     1.���A�����ҽX
        ///     2.���a���ҽX
        /// �|�o�ͦ��T���q�`�O�]��client�Pserver�������ۮe�ҭP.
        /// </summary>
        event Action<byte[], byte[]> ErrorVerifyEvent;
    }
}