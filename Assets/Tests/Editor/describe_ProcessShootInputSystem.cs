﻿using NSpec;
using Entitas;
using UnityEngine;

public class describe_ProcessShootInputSystem : nspec {

    void when_input_emmited() {

        it["create a bullet at player's position"] = () => {

            // given
            var inputPool = new Pool(InputComponentIds.TotalComponents);
            var corePool = new Pool(CoreComponentIds.TotalComponents);
            var bulletsPool = new Pool(BulletsComponentIds.TotalComponents);

            var inputSystem = new ProcessShootInputSystem(corePool, bulletsPool);

            var system = (IExecuteSystem)inputPool.CreateSystem(inputSystem);

            var player1 = corePool.CreateEntity()
                .AddPosition(new Vector3(1, 1, 1))
                .AddPlayer("Player1");

            corePool.CreateEntity()
                .AddPosition(new Vector3(2, 2, 2))
                .AddPlayer("Player2");

            inputPool.CreateEntity()
                .IsShootInput(true)
                .AddInputOwner("Player1");

            // when
            system.Execute();

            // then
            var bullet = bulletsPool.GetEntities(BulletsMatcher.Bullet).SingleEntity();
            bullet.should_not_be_null();
            bullet.position.value.should_be(player1.position.value);
            bullet.gameObjectObjectPool.pool.should_not_be_null();
            bullet.hasVelocity.should_be_true();

            inputPool.GetEntities(InputMatcher.ShootInput).Length.should_be(0);
        };
    }
}