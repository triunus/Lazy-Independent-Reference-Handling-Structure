using System;

namespace Foundations.Patterns.Singleton
{
    /// <summary>
    /// ���׸� ����� ��ӿ� �̱��� Ŭ����.
    /// private �����ڵ� �����ϸ�, ���� ���� �� OnInitialize() ȣ��.
    /// </summary>
    public abstract class Singleton<T> where T : class
    {
        /// <summary>
        /// Lazy<T>�� ����Ͽ� thread-safe�ϸ�, ó�� Instance�� ������ ���� �����ȴ�.
        /// </summary>
        private static readonly Lazy<T> lazyInstance = new(() =>
        {
            // private �Ű����� ���� ������ ȣ��
            T instance = (T)Activator.CreateInstance(typeof(T), nonPublic: true)!;

            // �ʱ�ȭ �޼��尡 �ִٸ� ȣ��
            if (instance is Singleton<T> singleton)
            {
                singleton.OnInitialize();
            }

            return instance;
        });

        /// <summary>
        /// �ܺο��� ���� ������ �̱� �ν��Ͻ�.
        /// </summary>
        public static T Instance => lazyInstance.Value;

        /// <summary>
        /// �ܺ� ���� ���ܿ� �⺻ ������.
        /// �ݵ�� private �Ǵ� protected�� �����ؾ� ��.
        /// </summary>
        protected Singleton() { }

        /// <summary>
        /// �ν��Ͻ� ���� ���� 1ȸ ȣ��Ǵ� �ʱ�ȭ �޼���.
        /// ��� Ŭ�������� override ����.
        /// </summary>
        protected virtual void OnInitialize() { }
    }
}