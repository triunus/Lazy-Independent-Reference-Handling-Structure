using System.Collections;
using UnityEngine;

using Foundations.ReferencesHandler;

namespace GameSystem.UnitSystem.PlayerSpawner
{
    public interface IPlayerSpawner
    {
        public IEnumerator SpawnPlayerUnit(int stageID);
    }

    public class PlayerSpawner : MonoBehaviour, IPlayerSpawner
    {
        private PlayerSpawnDBHandler PlayerSpawnDBHandler;
        private PlayerUnitDBHandler PlayerUnitDBHandler;

        [SerializeField] private Transform PlayerGameObjectParent;

        // Handler Manager를 통해 
        // 기능 수행을 위한 StaticHandler 참조. + 외부에서 기능 참조를 위해 자신의 Interface를 DynamicHandler에 등록.
        private void Awake()
        {
            var HandlerManager = LazyReferenceHandlerManager.Instance;
            this.PlayerSpawnDBHandler = HandlerManager.GetStaticHandler<PlayerSpawnDBHandler>();
            this.PlayerUnitDBHandler = HandlerManager.GetStaticHandler<PlayerUnitDBHandler>();

            HandlerManager.GetDynamicHandler<UnitSystemHandler>().IPlayerSpawner = this;
        }

        // 유닛 생성 작업.
        // StageID에 대응되는 생성 데이터 탐색.
        // 생성 UnitID에 대응되는 유닛 리소스 데이터 탐색.
        // 생성, 위치 지정, 생성 작업 대기.
        public IEnumerator SpawnPlayerUnit(int stageID)
        {
            if(!this.PlayerSpawnDBHandler.TryGetUnitSpawnDataGroup(stageID, out var unitSpawnDataGroup))
            {
                Debug.LogError($"Stage 생성 Player 정보에 오류 있음.");
            }

            foreach (var spawnData in unitSpawnDataGroup.UnitSpawnDatas)
            {
                if(!this.PlayerUnitDBHandler.TryGetUnitResourceData(spawnData.UnitID, out var unitResourceData))
                {
                    Debug.LogError($"UnitID랑 대응되는 Resoucrec들에 오류 있음.");
                }

                GameObject spawnedUnit = Instantiate(unitResourceData.UnitGameObject, this.PlayerGameObjectParent);
                spawnedUnit.transform.position = spawnData.SpawnPosition;

                // 생성 대기
                yield return null;

                yield return StartCoroutine(spawnedUnit.GetComponent<UnitManager.IUnitManager>().SpawnOperation());
            }
        }
    }
}
