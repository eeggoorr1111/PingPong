using UnityEngine;
using Zenject;
using PingPong.View;


namespace PingPong
{
    public class PingPongInstaller : MonoInstaller
    {
        [SerializeField] private PingPongView _view;


        public override void InstallBindings()
        {
            Container.Bind<PingPongView>().FromInstance(_view).AsSingle();
        }
    }
}
