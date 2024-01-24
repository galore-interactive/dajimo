using GONet;
using System.Collections.Generic;
using UnityEngine;

namespace DATS
{
    [RequireComponent(typeof(GONetLocal))]
    public class PlayerLocalContext : GONetParticipantCompanionBehaviour, INetworkEntity
    {
        private readonly List<PlayerController> _myPlayerControllers = new List<PlayerController>();

        [GONetAutoMagicalSync("TeamColor")] public byte currentTeamColor;

        [GONetAutoMagicalSync("TeamColor")] public bool _isBipedInRagdollState;

        /// <summary>
        /// For getting value with more appropriate typing, use <see cref="CurrentPlayerType"/> instead;
        /// </summary>
        [GONetAutoMagicalSync("TeamColor")] public byte _currentPlayerType;
        public PlayableEntityType CurrentPlayerType
        {
            get => (PlayableEntityType) _currentPlayerType;
            set => _currentPlayerType = (byte)value;
        }

        private static PlayerLocalContext _mine;
        public static PlayerLocalContext Mine => _mine ?? (_mine = GONetMain.MyLocal.GetComponent<PlayerLocalContext>());

        public static PlayerLocalContext LookupByAuthorityId(ushort authorityId)
        {
            GONetLocal local = GONetLocal.LookupByAuthorityId[authorityId];
            if (local == null)
            {
                GONetLog.Error($"Expecting to find GONetLocal for authorityId: {authorityId} but did not.  Therefore, cannot find PlayerLocalContext as desired.");
                return default;
            }

            return local.GetComponent<PlayerLocalContext>();
        }

        public override void OnGONetParticipantEnabled(GONetParticipant gonetParticipant)
        {
            base.OnGONetParticipantEnabled(gonetParticipant);

            //GONetLog.Debug($"RELATED TO 'receive entity event type' here is a newly enabled GNP.id: {gonetParticipant.GONetId} mine? {gonetParticipant.IsMine}");

            if (gonetParticipant.OwnerAuthorityId != this.gonetParticipant.OwnerAuthorityId) return;

            PlayerController playerController = gonetParticipant.GetComponent<PlayerController>();
            if (playerController)
            {
                _myPlayerControllers.Add(playerController);
                //GONetLog.Debug($"authId: {gonetParticipant.OwnerAuthorityId} _isBipedInRagdollState: {_isBipedInRagdollState} playerCtrl#: {_myPlayerControllers.Count}");

                if (_isBipedInRagdollState)
                {
                    playerController.ForceActivateRagdoll();
                }
            }
        }

        public override void OnGONetParticipantDeserializeInitAllCompleted()
        {
            base.OnGONetParticipantDeserializeInitAllCompleted();

            //GONetLog.Debug($"authId: {gonetParticipant.OwnerAuthorityId} _isBipedInRagdollState: {_isBipedInRagdollState} playerCtrl#: {_myPlayerControllers.Count}");
            /* deemed unnecessary after other changes made in this commit, but was a thought
            foreach (var myPlayerController in _myPlayerControllers)
            {
                myPlayerController.UpdateAllMyColorRelatedValues(currentTeamColor);
            }
            */
        }

        public override void OnGONetParticipantDisabled(GONetParticipant gonetParticipant)
        {
            base.OnGONetParticipantDisabled(gonetParticipant);

            if (!gonetParticipant.IsMine) return;

            PlayerController playerController = gonetParticipant.GetComponent<PlayerController>();
            if (playerController)
            {
                _myPlayerControllers.Remove(playerController);
            }
        }

        #region INetworkEntity
        public bool DoReceiveEntityStates()
        {
            //_myPlayerControllers.ForEach(x => { if (x) x.DoReceiveEntityStates(); });
            return true;
        }

        public ushort GetClientOwnerId()
        {
            return GONetParticipant.OwnerAuthorityId;
        }

        public EntityState GetEntityStateWithoutEvent()
        {
            return default; // TODO FIXME implement
        }

        public uint GetNetworkEntityId()
        {
            return GONetParticipant.GONetId;
        }

        public GONetParticipant GetNetworkObject()
        {
            return GONetParticipant;
        }

        public bool IsPlayer()
        {
            return false; // TODO FIXME implement
        }

        public void ReceiveEntityEvent(LocalContextEntityEvent entityEvent)
        {
            GONetLog.Debug($"receive entity event type: {entityEvent.type} my player ctrl #: {_myPlayerControllers.Count} isMine: {GONetParticipant.IsMine}");
            _myPlayerControllers.ForEach(x => { if (x) x.ReceiveEntityEvent(entityEvent); });
        }

        public void ReceiveEntityState(EntityState entityState)
        {
            _myPlayerControllers.ForEach(x => { if (x) x.ReceiveEntityState(entityState); });
        }

        public void Server_SetCollidersVisibility(bool visibility)
        {
            _myPlayerControllers.ForEach(x => { if (x) x.Server_SetCollidersVisibility(visibility); });
        }
        #endregion
    }
}
