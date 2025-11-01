


/* GONet (TM, serial number 88592370), Copyright (c) 2019-2023 Galore Interactive LLC - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential, email: contactus@galoreinteractive.com
 * 
 *
 * Authorized use is explicitly limited to the following:	
 * -The ability to view and reference source code without changing it
 * -The ability to enhance debugging with source code access
 * -The ability to distribute products based on original sources for non-commercial purposes, whereas this license must be included if source code provided in said products
 * -The ability to commercialize products built on original source code, whereas this license must be included if source code provided in said products and whereas the products are interactive multi-player video games and cannot be viewed as a product competitive to GONet
 * -The ability to modify source code for local use only
 * -The ability to distribute products based on modified sources for non-commercial purposes, whereas this license must be included if source code provided in said products
 * -The ability to commercialize products built on modified source code, whereas this license must be included if source code provided in said products and whereas the products are interactive multi-player video games and cannot be viewed as a product competitive to GONet
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using GONet;

namespace GONet
{

	[MemoryPack.MemoryPackUnion(0, typeof(BipedInputState))]
		[MemoryPack.MemoryPackUnion(1, typeof(DroneInputsState))]
		[MemoryPack.MemoryPackUnion(2, typeof(GONet.AutoMagicalSync_AllCurrentValues_Message))]
		[MemoryPack.MemoryPackUnion(3, typeof(GONet.AutoMagicalSync_ValueChanges_Message))]
		[MemoryPack.MemoryPackUnion(4, typeof(GONet.AutoMagicalSync_ValuesNowAtRest_Message))]
		[MemoryPack.MemoryPackUnion(5, typeof(GONet.ClientRemotelyControlledGONetIdServerBatchAssignmentEvent))]
		[MemoryPack.MemoryPackUnion(6, typeof(GONet.ClientRemotelyControlledGONetIdServerBatchRequestEvent))]
		[MemoryPack.MemoryPackUnion(7, typeof(GONet.ClientStateChangedEvent))]
		[MemoryPack.MemoryPackUnion(8, typeof(GONet.ClientTypeFlagsChangedEvent))]
		[MemoryPack.MemoryPackUnion(9, typeof(GONet.DespawnGONetParticipantEvent))]
		[MemoryPack.MemoryPackUnion(10, typeof(GONet.GONetParticipantDeserializeInitAllCompletedEvent))]
		[MemoryPack.MemoryPackUnion(11, typeof(GONet.GONetParticipantDisabledEvent))]
		[MemoryPack.MemoryPackUnion(12, typeof(GONet.GONetParticipantEnabledEvent))]
		[MemoryPack.MemoryPackUnion(13, typeof(GONet.GONetParticipantStartedEvent))]
		[MemoryPack.MemoryPackUnion(14, typeof(GONet.InstantiateGONetParticipantEvent))]
		[MemoryPack.MemoryPackUnion(15, typeof(GONet.InternalOnlyMemoryPackComilationAssistanceForGenerated))]
		[MemoryPack.MemoryPackUnion(16, typeof(GONet.OwnerAuthorityIdAssignmentEvent))]
		[MemoryPack.MemoryPackUnion(17, typeof(GONet.PersistentEvents_Bundle))]
		[MemoryPack.MemoryPackUnion(18, typeof(GONet.PersistentEvents_BundleChunk))]
		[MemoryPack.MemoryPackUnion(19, typeof(GONet.PersistentRoutedRpcEvent))]
		[MemoryPack.MemoryPackUnion(20, typeof(GONet.PersistentRpcEvent))]
		[MemoryPack.MemoryPackUnion(21, typeof(GONet.RemoteClientStateChangedEvent))]
		[MemoryPack.MemoryPackUnion(22, typeof(GONet.RequestMessage))]
		[MemoryPack.MemoryPackUnion(23, typeof(GONet.ResponseMessage))]
		[MemoryPack.MemoryPackUnion(24, typeof(GONet.RoutedRpcEvent))]
		[MemoryPack.MemoryPackUnion(25, typeof(GONet.RpcDeliveryReportEvent))]
		[MemoryPack.MemoryPackUnion(26, typeof(GONet.RpcEvent))]
		[MemoryPack.MemoryPackUnion(27, typeof(GONet.RpcResponseEvent))]
		[MemoryPack.MemoryPackUnion(28, typeof(GONet.SceneLoadCompleteEvent))]
		[MemoryPack.MemoryPackUnion(29, typeof(GONet.SceneLoadEvent))]
		[MemoryPack.MemoryPackUnion(30, typeof(GONet.SceneUnloadEvent))]
		[MemoryPack.MemoryPackUnion(31, typeof(GONet.ServerSaysClientInitializationCompletion))]
		[MemoryPack.MemoryPackUnion(32, typeof(GONet.SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId))]
		[MemoryPack.MemoryPackUnion(33, typeof(GONet.SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount))]
		[MemoryPack.MemoryPackUnion(34, typeof(GONet.SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles))]
		[MemoryPack.MemoryPackUnion(35, typeof(GONet.SyncEvent_EntityStateSynchronizer_isCrouched))]
		[MemoryPack.MemoryPackUnion(36, typeof(GONet.SyncEvent_EntityStateSynchronizer_isGrounded))]
		[MemoryPack.MemoryPackUnion(37, typeof(GONet.SyncEvent_EntityStateSynchronizer_movementInput))]
		[MemoryPack.MemoryPackUnion(38, typeof(GONet.SyncEvent_EntityStateSynchronizer_targetEntityId))]
		[MemoryPack.MemoryPackUnion(39, typeof(GONet.SyncEvent_EntityStateSynchronizer_velocityVector))]
		[MemoryPack.MemoryPackUnion(40, typeof(GONet.SyncEvent_GONetParticipant_GONetId))]
		[MemoryPack.MemoryPackUnion(41, typeof(GONet.SyncEvent_GONetParticipant_IsPositionSyncd))]
		[MemoryPack.MemoryPackUnion(42, typeof(GONet.SyncEvent_GONetParticipant_IsRotationSyncd))]
		[MemoryPack.MemoryPackUnion(43, typeof(GONet.SyncEvent_GONetParticipant_OwnerAuthorityId))]
		[MemoryPack.MemoryPackUnion(44, typeof(GONet.SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId))]
		[MemoryPack.MemoryPackUnion(45, typeof(GONet.SyncEvent_PlayerController_splitRotation))]
		[MemoryPack.MemoryPackUnion(46, typeof(GONet.SyncEvent_PlayerLocalContext__currentPlayerType))]
		[MemoryPack.MemoryPackUnion(47, typeof(GONet.SyncEvent_PlayerLocalContext__isBipedInRagdollState))]
		[MemoryPack.MemoryPackUnion(48, typeof(GONet.SyncEvent_PlayerLocalContext_currentTeamColor))]
		[MemoryPack.MemoryPackUnion(49, typeof(GONet.SyncEvent_Time_ElapsedTicks_SetFromAuthority))]
		[MemoryPack.MemoryPackUnion(50, typeof(GONet.SyncEvent_Transform_position))]
		[MemoryPack.MemoryPackUnion(51, typeof(GONet.SyncEvent_Transform_rotation))]
		[MemoryPack.MemoryPackUnion(52, typeof(GONet.ValueMonitoringSupport_BaselineExpiredEvent))]
		[MemoryPack.MemoryPackUnion(53, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Boolean))]
		[MemoryPack.MemoryPackUnion(54, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Byte))]
		[MemoryPack.MemoryPackUnion(55, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Double))]
		[MemoryPack.MemoryPackUnion(56, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Int16))]
		[MemoryPack.MemoryPackUnion(57, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Int32))]
		[MemoryPack.MemoryPackUnion(58, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Int64))]
		[MemoryPack.MemoryPackUnion(59, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_SByte))]
		[MemoryPack.MemoryPackUnion(60, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Single))]
		[MemoryPack.MemoryPackUnion(61, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_UInt16))]
		[MemoryPack.MemoryPackUnion(62, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_UInt32))]
		[MemoryPack.MemoryPackUnion(63, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_UInt64))]
		[MemoryPack.MemoryPackUnion(64, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Quaternion))]
		[MemoryPack.MemoryPackUnion(65, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector2))]
		[MemoryPack.MemoryPackUnion(66, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector3))]
		[MemoryPack.MemoryPackUnion(67, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector4))]
		[MemoryPack.MemoryPackUnion(68, typeof(InputPacket))]
		[MemoryPack.MemoryPackUnion(69, typeof(InputStateEvent))]
		[MemoryPack.MemoryPackUnion(70, typeof(LocalContextEntityEvent))]
		[MemoryPack.MemoryPackUnion(71, typeof(MyEvent))]
		[MemoryPack.MemoryPackUnion(72, typeof(SnapshotEvent))]
		[MemoryPack.MemoryPackUnion(73, typeof(SnapshotState))]
		[MemoryPack.MemoryPackable] public partial interface IGONetEvent { }


	[MemoryPack.MemoryPackUnion(0, typeof(BipedInputState))]
		[MemoryPack.MemoryPackUnion(1, typeof(DroneInputsState))]
		[MemoryPack.MemoryPackUnion(2, typeof(GONet.AutoMagicalSync_AllCurrentValues_Message))]
		[MemoryPack.MemoryPackUnion(3, typeof(GONet.AutoMagicalSync_ValueChanges_Message))]
		[MemoryPack.MemoryPackUnion(4, typeof(GONet.AutoMagicalSync_ValuesNowAtRest_Message))]
		[MemoryPack.MemoryPackUnion(5, typeof(GONet.ClientRemotelyControlledGONetIdServerBatchAssignmentEvent))]
		[MemoryPack.MemoryPackUnion(6, typeof(GONet.ClientRemotelyControlledGONetIdServerBatchRequestEvent))]
		[MemoryPack.MemoryPackUnion(7, typeof(GONet.ClientStateChangedEvent))]
		[MemoryPack.MemoryPackUnion(8, typeof(GONet.ClientTypeFlagsChangedEvent))]
		[MemoryPack.MemoryPackUnion(9, typeof(GONet.GONetParticipantDeserializeInitAllCompletedEvent))]
		[MemoryPack.MemoryPackUnion(10, typeof(GONet.GONetParticipantDisabledEvent))]
		[MemoryPack.MemoryPackUnion(11, typeof(GONet.GONetParticipantEnabledEvent))]
		[MemoryPack.MemoryPackUnion(12, typeof(GONet.GONetParticipantStartedEvent))]
		[MemoryPack.MemoryPackUnion(13, typeof(GONet.InternalOnlyMemoryPackComilationAssistanceForGenerated))]
		[MemoryPack.MemoryPackUnion(14, typeof(GONet.PersistentEvents_Bundle))]
		[MemoryPack.MemoryPackUnion(15, typeof(GONet.PersistentEvents_BundleChunk))]
		[MemoryPack.MemoryPackUnion(16, typeof(GONet.RemoteClientStateChangedEvent))]
		[MemoryPack.MemoryPackUnion(17, typeof(GONet.RequestMessage))]
		[MemoryPack.MemoryPackUnion(18, typeof(GONet.ResponseMessage))]
		[MemoryPack.MemoryPackUnion(19, typeof(GONet.RoutedRpcEvent))]
		[MemoryPack.MemoryPackUnion(20, typeof(GONet.RpcDeliveryReportEvent))]
		[MemoryPack.MemoryPackUnion(21, typeof(GONet.RpcEvent))]
		[MemoryPack.MemoryPackUnion(22, typeof(GONet.RpcResponseEvent))]
		[MemoryPack.MemoryPackUnion(23, typeof(GONet.SceneLoadCompleteEvent))]
		[MemoryPack.MemoryPackUnion(24, typeof(GONet.ServerSaysClientInitializationCompletion))]
		[MemoryPack.MemoryPackUnion(25, typeof(GONet.SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId))]
		[MemoryPack.MemoryPackUnion(26, typeof(GONet.SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount))]
		[MemoryPack.MemoryPackUnion(27, typeof(GONet.SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles))]
		[MemoryPack.MemoryPackUnion(28, typeof(GONet.SyncEvent_EntityStateSynchronizer_isCrouched))]
		[MemoryPack.MemoryPackUnion(29, typeof(GONet.SyncEvent_EntityStateSynchronizer_isGrounded))]
		[MemoryPack.MemoryPackUnion(30, typeof(GONet.SyncEvent_EntityStateSynchronizer_movementInput))]
		[MemoryPack.MemoryPackUnion(31, typeof(GONet.SyncEvent_EntityStateSynchronizer_targetEntityId))]
		[MemoryPack.MemoryPackUnion(32, typeof(GONet.SyncEvent_EntityStateSynchronizer_velocityVector))]
		[MemoryPack.MemoryPackUnion(33, typeof(GONet.SyncEvent_GONetParticipant_GONetId))]
		[MemoryPack.MemoryPackUnion(34, typeof(GONet.SyncEvent_GONetParticipant_IsPositionSyncd))]
		[MemoryPack.MemoryPackUnion(35, typeof(GONet.SyncEvent_GONetParticipant_IsRotationSyncd))]
		[MemoryPack.MemoryPackUnion(36, typeof(GONet.SyncEvent_GONetParticipant_OwnerAuthorityId))]
		[MemoryPack.MemoryPackUnion(37, typeof(GONet.SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId))]
		[MemoryPack.MemoryPackUnion(38, typeof(GONet.SyncEvent_PlayerController_splitRotation))]
		[MemoryPack.MemoryPackUnion(39, typeof(GONet.SyncEvent_PlayerLocalContext__currentPlayerType))]
		[MemoryPack.MemoryPackUnion(40, typeof(GONet.SyncEvent_PlayerLocalContext__isBipedInRagdollState))]
		[MemoryPack.MemoryPackUnion(41, typeof(GONet.SyncEvent_PlayerLocalContext_currentTeamColor))]
		[MemoryPack.MemoryPackUnion(42, typeof(GONet.SyncEvent_Time_ElapsedTicks_SetFromAuthority))]
		[MemoryPack.MemoryPackUnion(43, typeof(GONet.SyncEvent_Transform_position))]
		[MemoryPack.MemoryPackUnion(44, typeof(GONet.SyncEvent_Transform_rotation))]
		[MemoryPack.MemoryPackUnion(45, typeof(InputPacket))]
		[MemoryPack.MemoryPackUnion(46, typeof(InputStateEvent))]
		[MemoryPack.MemoryPackUnion(47, typeof(LocalContextEntityEvent))]
		[MemoryPack.MemoryPackUnion(48, typeof(MyEvent))]
		[MemoryPack.MemoryPackUnion(49, typeof(SnapshotEvent))]
		[MemoryPack.MemoryPackUnion(50, typeof(SnapshotState))]
		[MemoryPack.MemoryPackable] public partial interface ITransientEvent : IGONetEvent { }


	[MemoryPack.MemoryPackUnion(0, typeof(GONet.DespawnGONetParticipantEvent))]
		[MemoryPack.MemoryPackUnion(1, typeof(GONet.InstantiateGONetParticipantEvent))]
		[MemoryPack.MemoryPackUnion(2, typeof(GONet.OwnerAuthorityIdAssignmentEvent))]
		[MemoryPack.MemoryPackUnion(3, typeof(GONet.PersistentRoutedRpcEvent))]
		[MemoryPack.MemoryPackUnion(4, typeof(GONet.PersistentRpcEvent))]
		[MemoryPack.MemoryPackUnion(5, typeof(GONet.SceneLoadEvent))]
		[MemoryPack.MemoryPackUnion(6, typeof(GONet.SceneUnloadEvent))]
		[MemoryPack.MemoryPackUnion(7, typeof(GONet.ValueMonitoringSupport_BaselineExpiredEvent))]
		[MemoryPack.MemoryPackUnion(8, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Boolean))]
		[MemoryPack.MemoryPackUnion(9, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Byte))]
		[MemoryPack.MemoryPackUnion(10, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Double))]
		[MemoryPack.MemoryPackUnion(11, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Int16))]
		[MemoryPack.MemoryPackUnion(12, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Int32))]
		[MemoryPack.MemoryPackUnion(13, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Int64))]
		[MemoryPack.MemoryPackUnion(14, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_SByte))]
		[MemoryPack.MemoryPackUnion(15, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Single))]
		[MemoryPack.MemoryPackUnion(16, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_UInt16))]
		[MemoryPack.MemoryPackUnion(17, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_UInt32))]
		[MemoryPack.MemoryPackUnion(18, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_UInt64))]
		[MemoryPack.MemoryPackUnion(19, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Quaternion))]
		[MemoryPack.MemoryPackUnion(20, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector2))]
		[MemoryPack.MemoryPackUnion(21, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector3))]
		[MemoryPack.MemoryPackUnion(22, typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector4))]
		[MemoryPack.MemoryPackable] public partial interface IPersistentEvent : IGONetEvent { }

		[MemoryPack.MemoryPackUnion(0, typeof(GONet.SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId))]
		[MemoryPack.MemoryPackUnion(1, typeof(GONet.SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount))]
		[MemoryPack.MemoryPackUnion(2, typeof(GONet.SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles))]
		[MemoryPack.MemoryPackUnion(3, typeof(GONet.SyncEvent_EntityStateSynchronizer_isCrouched))]
		[MemoryPack.MemoryPackUnion(4, typeof(GONet.SyncEvent_EntityStateSynchronizer_isGrounded))]
		[MemoryPack.MemoryPackUnion(5, typeof(GONet.SyncEvent_EntityStateSynchronizer_movementInput))]
		[MemoryPack.MemoryPackUnion(6, typeof(GONet.SyncEvent_EntityStateSynchronizer_targetEntityId))]
		[MemoryPack.MemoryPackUnion(7, typeof(GONet.SyncEvent_EntityStateSynchronizer_velocityVector))]
		[MemoryPack.MemoryPackUnion(8, typeof(GONet.SyncEvent_GONetParticipant_GONetId))]
		[MemoryPack.MemoryPackUnion(9, typeof(GONet.SyncEvent_GONetParticipant_IsPositionSyncd))]
		[MemoryPack.MemoryPackUnion(10, typeof(GONet.SyncEvent_GONetParticipant_IsRotationSyncd))]
		[MemoryPack.MemoryPackUnion(11, typeof(GONet.SyncEvent_GONetParticipant_OwnerAuthorityId))]
		[MemoryPack.MemoryPackUnion(12, typeof(GONet.SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId))]
		[MemoryPack.MemoryPackUnion(13, typeof(GONet.SyncEvent_PlayerController_splitRotation))]
		[MemoryPack.MemoryPackUnion(14, typeof(GONet.SyncEvent_PlayerLocalContext__currentPlayerType))]
		[MemoryPack.MemoryPackUnion(15, typeof(GONet.SyncEvent_PlayerLocalContext__isBipedInRagdollState))]
		[MemoryPack.MemoryPackUnion(16, typeof(GONet.SyncEvent_PlayerLocalContext_currentTeamColor))]
		[MemoryPack.MemoryPackUnion(17, typeof(GONet.SyncEvent_Transform_position))]
		[MemoryPack.MemoryPackUnion(18, typeof(GONet.SyncEvent_Transform_rotation))]
		[MemoryPack.MemoryPackUnion(19, typeof(GONet.SyncEvent_Time_ElapsedTicks_SetFromAuthority))]
		public abstract partial class SyncEvent_ValueChangeProcessed { }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_GONetParticipant_GONetId : SyncEvent_ValueChangeProcessed
    {
		public System.UInt32 valuePrevious;
		public System.UInt32 valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_GONetParticipant_GONetId;

		static readonly Utils.ObjectPool<SyncEvent_GONetParticipant_GONetId> pool = new Utils.ObjectPool<SyncEvent_GONetParticipant_GONetId>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_GONetParticipant_GONetId> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_GONetParticipant_GONetId>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.UInt32, System.UInt32)"/>.
        /// </summary>
        public SyncEvent_GONetParticipant_GONetId() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_GONetParticipant_GONetId)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_GONetParticipant_GONetId Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.UInt32 valuePrevious, System.UInt32 valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_GONetParticipant_GONetId autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_GONetParticipant_GONetId borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_GONetParticipant_IsPositionSyncd : SyncEvent_ValueChangeProcessed
    {
		public System.Boolean valuePrevious;
		public System.Boolean valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_GONetParticipant_IsPositionSyncd;

		static readonly Utils.ObjectPool<SyncEvent_GONetParticipant_IsPositionSyncd> pool = new Utils.ObjectPool<SyncEvent_GONetParticipant_IsPositionSyncd>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_GONetParticipant_IsPositionSyncd> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_GONetParticipant_IsPositionSyncd>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.Boolean, System.Boolean)"/>.
        /// </summary>
        public SyncEvent_GONetParticipant_IsPositionSyncd() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_GONetParticipant_IsPositionSyncd)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_GONetParticipant_IsPositionSyncd Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.Boolean valuePrevious, System.Boolean valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_GONetParticipant_IsPositionSyncd autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_GONetParticipant_IsPositionSyncd borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_GONetParticipant_IsRotationSyncd : SyncEvent_ValueChangeProcessed
    {
		public System.Boolean valuePrevious;
		public System.Boolean valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_GONetParticipant_IsRotationSyncd;

		static readonly Utils.ObjectPool<SyncEvent_GONetParticipant_IsRotationSyncd> pool = new Utils.ObjectPool<SyncEvent_GONetParticipant_IsRotationSyncd>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_GONetParticipant_IsRotationSyncd> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_GONetParticipant_IsRotationSyncd>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.Boolean, System.Boolean)"/>.
        /// </summary>
        public SyncEvent_GONetParticipant_IsRotationSyncd() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_GONetParticipant_IsRotationSyncd)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_GONetParticipant_IsRotationSyncd Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.Boolean valuePrevious, System.Boolean valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_GONetParticipant_IsRotationSyncd autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_GONetParticipant_IsRotationSyncd borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_GONetParticipant_OwnerAuthorityId : SyncEvent_ValueChangeProcessed
    {
		public System.UInt16 valuePrevious;
		public System.UInt16 valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_GONetParticipant_OwnerAuthorityId;

		static readonly Utils.ObjectPool<SyncEvent_GONetParticipant_OwnerAuthorityId> pool = new Utils.ObjectPool<SyncEvent_GONetParticipant_OwnerAuthorityId>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_GONetParticipant_OwnerAuthorityId> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_GONetParticipant_OwnerAuthorityId>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.UInt16, System.UInt16)"/>.
        /// </summary>
        public SyncEvent_GONetParticipant_OwnerAuthorityId() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_GONetParticipant_OwnerAuthorityId)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_GONetParticipant_OwnerAuthorityId Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.UInt16 valuePrevious, System.UInt16 valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_GONetParticipant_OwnerAuthorityId autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_GONetParticipant_OwnerAuthorityId borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId : SyncEvent_ValueChangeProcessed
    {
		public System.UInt16 valuePrevious;
		public System.UInt16 valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId;

		static readonly Utils.ObjectPool<SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId> pool = new Utils.ObjectPool<SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.UInt16, System.UInt16)"/>.
        /// </summary>
        public SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.UInt16 valuePrevious, System.UInt16 valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_Transform_rotation : SyncEvent_ValueChangeProcessed
    {
		public UnityEngine.Quaternion valuePrevious;
		public UnityEngine.Quaternion valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_Transform_rotation;

		static readonly Utils.ObjectPool<SyncEvent_Transform_rotation> pool = new Utils.ObjectPool<SyncEvent_Transform_rotation>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_Transform_rotation> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_Transform_rotation>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, UnityEngine.Quaternion, UnityEngine.Quaternion)"/>.
        /// </summary>
        public SyncEvent_Transform_rotation() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_Transform_rotation)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_Transform_rotation Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, UnityEngine.Quaternion valuePrevious, UnityEngine.Quaternion valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_Transform_rotation autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_Transform_rotation borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_Transform_position : SyncEvent_ValueChangeProcessed
    {
		public UnityEngine.Vector3 valuePrevious;
		public UnityEngine.Vector3 valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_Transform_position;

		static readonly Utils.ObjectPool<SyncEvent_Transform_position> pool = new Utils.ObjectPool<SyncEvent_Transform_position>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_Transform_position> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_Transform_position>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, UnityEngine.Vector3, UnityEngine.Vector3)"/>.
        /// </summary>
        public SyncEvent_Transform_position() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_Transform_position)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_Transform_position Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, UnityEngine.Vector3 valuePrevious, UnityEngine.Vector3 valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_Transform_position autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_Transform_position borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId : SyncEvent_ValueChangeProcessed
    {
		public System.UInt16 valuePrevious;
		public System.UInt16 valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId;

		static readonly Utils.ObjectPool<SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId> pool = new Utils.ObjectPool<SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.UInt16, System.UInt16)"/>.
        /// </summary>
        public SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.UInt16 valuePrevious, System.UInt16 valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount : SyncEvent_ValueChangeProcessed
    {
		public System.Int32 valuePrevious;
		public System.Int32 valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount;

		static readonly Utils.ObjectPool<SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount> pool = new Utils.ObjectPool<SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.Int32, System.Int32)"/>.
        /// </summary>
        public SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.Int32 valuePrevious, System.Int32 valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles : SyncEvent_ValueChangeProcessed
    {
		public UnityEngine.Vector3 valuePrevious;
		public UnityEngine.Vector3 valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles;

		static readonly Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles> pool = new Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, UnityEngine.Vector3, UnityEngine.Vector3)"/>.
        /// </summary>
        public SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, UnityEngine.Vector3 valuePrevious, UnityEngine.Vector3 valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_EntityStateSynchronizer_isCrouched : SyncEvent_ValueChangeProcessed
    {
		public System.Boolean valuePrevious;
		public System.Boolean valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_EntityStateSynchronizer_isCrouched;

		static readonly Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_isCrouched> pool = new Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_isCrouched>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_isCrouched> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_isCrouched>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.Boolean, System.Boolean)"/>.
        /// </summary>
        public SyncEvent_EntityStateSynchronizer_isCrouched() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_EntityStateSynchronizer_isCrouched)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_EntityStateSynchronizer_isCrouched Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.Boolean valuePrevious, System.Boolean valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_EntityStateSynchronizer_isCrouched autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_EntityStateSynchronizer_isCrouched borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_EntityStateSynchronizer_isGrounded : SyncEvent_ValueChangeProcessed
    {
		public System.Boolean valuePrevious;
		public System.Boolean valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_EntityStateSynchronizer_isGrounded;

		static readonly Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_isGrounded> pool = new Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_isGrounded>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_isGrounded> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_isGrounded>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.Boolean, System.Boolean)"/>.
        /// </summary>
        public SyncEvent_EntityStateSynchronizer_isGrounded() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_EntityStateSynchronizer_isGrounded)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_EntityStateSynchronizer_isGrounded Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.Boolean valuePrevious, System.Boolean valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_EntityStateSynchronizer_isGrounded autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_EntityStateSynchronizer_isGrounded borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_EntityStateSynchronizer_movementInput : SyncEvent_ValueChangeProcessed
    {
		public UnityEngine.Vector2 valuePrevious;
		public UnityEngine.Vector2 valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_EntityStateSynchronizer_movementInput;

		static readonly Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_movementInput> pool = new Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_movementInput>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_movementInput> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_movementInput>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, UnityEngine.Vector2, UnityEngine.Vector2)"/>.
        /// </summary>
        public SyncEvent_EntityStateSynchronizer_movementInput() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_EntityStateSynchronizer_movementInput)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_EntityStateSynchronizer_movementInput Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, UnityEngine.Vector2 valuePrevious, UnityEngine.Vector2 valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_EntityStateSynchronizer_movementInput autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_EntityStateSynchronizer_movementInput borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_EntityStateSynchronizer_targetEntityId : SyncEvent_ValueChangeProcessed
    {
		public System.UInt32 valuePrevious;
		public System.UInt32 valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_EntityStateSynchronizer_targetEntityId;

		static readonly Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_targetEntityId> pool = new Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_targetEntityId>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_targetEntityId> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_targetEntityId>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.UInt32, System.UInt32)"/>.
        /// </summary>
        public SyncEvent_EntityStateSynchronizer_targetEntityId() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_EntityStateSynchronizer_targetEntityId)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_EntityStateSynchronizer_targetEntityId Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.UInt32 valuePrevious, System.UInt32 valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_EntityStateSynchronizer_targetEntityId autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_EntityStateSynchronizer_targetEntityId borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_EntityStateSynchronizer_velocityVector : SyncEvent_ValueChangeProcessed
    {
		public UnityEngine.Vector3 valuePrevious;
		public UnityEngine.Vector3 valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_EntityStateSynchronizer_velocityVector;

		static readonly Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_velocityVector> pool = new Utils.ObjectPool<SyncEvent_EntityStateSynchronizer_velocityVector>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_velocityVector> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_EntityStateSynchronizer_velocityVector>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, UnityEngine.Vector3, UnityEngine.Vector3)"/>.
        /// </summary>
        public SyncEvent_EntityStateSynchronizer_velocityVector() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_EntityStateSynchronizer_velocityVector)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_EntityStateSynchronizer_velocityVector Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, UnityEngine.Vector3 valuePrevious, UnityEngine.Vector3 valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_EntityStateSynchronizer_velocityVector autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_EntityStateSynchronizer_velocityVector borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_PlayerLocalContext__currentPlayerType : SyncEvent_ValueChangeProcessed
    {
		public System.Byte valuePrevious;
		public System.Byte valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_PlayerLocalContext__currentPlayerType;

		static readonly Utils.ObjectPool<SyncEvent_PlayerLocalContext__currentPlayerType> pool = new Utils.ObjectPool<SyncEvent_PlayerLocalContext__currentPlayerType>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_PlayerLocalContext__currentPlayerType> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_PlayerLocalContext__currentPlayerType>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.Byte, System.Byte)"/>.
        /// </summary>
        public SyncEvent_PlayerLocalContext__currentPlayerType() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_PlayerLocalContext__currentPlayerType)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_PlayerLocalContext__currentPlayerType Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.Byte valuePrevious, System.Byte valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_PlayerLocalContext__currentPlayerType autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_PlayerLocalContext__currentPlayerType borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_PlayerLocalContext__isBipedInRagdollState : SyncEvent_ValueChangeProcessed
    {
		public System.Boolean valuePrevious;
		public System.Boolean valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_PlayerLocalContext__isBipedInRagdollState;

		static readonly Utils.ObjectPool<SyncEvent_PlayerLocalContext__isBipedInRagdollState> pool = new Utils.ObjectPool<SyncEvent_PlayerLocalContext__isBipedInRagdollState>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_PlayerLocalContext__isBipedInRagdollState> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_PlayerLocalContext__isBipedInRagdollState>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.Boolean, System.Boolean)"/>.
        /// </summary>
        public SyncEvent_PlayerLocalContext__isBipedInRagdollState() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_PlayerLocalContext__isBipedInRagdollState)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_PlayerLocalContext__isBipedInRagdollState Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.Boolean valuePrevious, System.Boolean valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_PlayerLocalContext__isBipedInRagdollState autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_PlayerLocalContext__isBipedInRagdollState borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_PlayerLocalContext_currentTeamColor : SyncEvent_ValueChangeProcessed
    {
		public System.Byte valuePrevious;
		public System.Byte valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_PlayerLocalContext_currentTeamColor;

		static readonly Utils.ObjectPool<SyncEvent_PlayerLocalContext_currentTeamColor> pool = new Utils.ObjectPool<SyncEvent_PlayerLocalContext_currentTeamColor>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_PlayerLocalContext_currentTeamColor> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_PlayerLocalContext_currentTeamColor>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, System.Byte, System.Byte)"/>.
        /// </summary>
        public SyncEvent_PlayerLocalContext_currentTeamColor() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_PlayerLocalContext_currentTeamColor)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_PlayerLocalContext_currentTeamColor Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, System.Byte valuePrevious, System.Byte valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_PlayerLocalContext_currentTeamColor autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_PlayerLocalContext_currentTeamColor borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

    
	
    /// <summary>
    /// <para>This represents that a sync value change has been processed. Use the class name to determine the related value/type/context.</para>
    /// <para>Please, do NOT reference this class in any of your codebases, this is GONet internal only. Do NOT subscribe to any of the SyncEvents (GONet.EventBus.Subscribe) using this class. Instead, use SyncEvent_GeneratedTypes.</para>
	/// <para>Two major occassions this event will occur (use this.<see cref="SyncEvent_ValueChangeProcessed.Explanation"/> to know which of the two occasions this represents):</para>
    /// <para>1) For an outbound change being sent to remote recipients (in which case, this event is published just AFTER the change has been sent to remote sources; however, the remote recipients likely have NOT received/processed it yet.)</para>
    /// <para>2) For an inbound change received from a remote source (in which case, this event is published just AFTER the change has been applied)</para>
    /// </summary>
    [MemoryPack.MemoryPackable]
    internal sealed partial class SyncEvent_PlayerController_splitRotation : SyncEvent_ValueChangeProcessed
    {
		public UnityEngine.Quaternion valuePrevious;
		public UnityEngine.Quaternion valueNew;
		public override GONetSyncableValue ValuePrevious => valuePrevious;
		public override GONetSyncableValue ValueNew => valueNew;
		public override GONet.SyncEvent_GeneratedTypes SyncEvent_GeneratedType => SyncEvent_GeneratedTypes.SyncEvent_PlayerController_splitRotation;

		static readonly Utils.ObjectPool<SyncEvent_PlayerController_splitRotation> pool = new Utils.ObjectPool<SyncEvent_PlayerController_splitRotation>(GONetMain.SYNC_EVENT_QUEUE_SAVE_WHEN_FULL_SIZE + 50, 1);
        static readonly System.Collections.Concurrent.ConcurrentQueue<SyncEvent_PlayerController_splitRotation> returnQueue_onceOnBorrowThread = new System.Collections.Concurrent.ConcurrentQueue<SyncEvent_PlayerController_splitRotation>();
        static System.Threading.Thread borrowThread;

        /// <summary>
        /// Do NOT use!  This is for object pooling and MemoryPack only.
        /// Instead, call <see cref="Borrow(SyncEvent_ValueChangeProcessedExplanation, long, uint, uint, byte, UnityEngine.Quaternion, UnityEngine.Quaternion)"/>.
        /// </summary>
        public SyncEvent_PlayerController_splitRotation() { }

        /// <summary>
        /// IMPORTANT: It is the caller's responsibility to ensure the instance returned from this method is also returned back
        ///            here (i.e., to private object pool) via <see cref="Return(SyncEvent_PlayerController_splitRotation)"/> when no longer needed!
        /// </summary>
        public static SyncEvent_PlayerController_splitRotation Borrow(SyncEvent_ValueChangeProcessedExplanation explanation, long occurredAtElapsedTicks, ushort relatedOwnerAuthorityId, uint gonetId, byte codeGenerationId, byte syncMemberIndex, UnityEngine.Quaternion valuePrevious, UnityEngine.Quaternion valueNew)
		{
            if (borrowThread == null)
            {
                borrowThread = System.Threading.Thread.CurrentThread;
            }
            else if (borrowThread != System.Threading.Thread.CurrentThread)
            {
                const string REQUIRED_CALL_SAME_BORROW_THREAD = "Not allowed to call this from more than one thread.  So, ensure Borrow() is called from the same exact thread for this specific event type.  NOTE: Each event type can have its' Borrow() called from a different thread from one another.";
                throw new InvalidOperationException(REQUIRED_CALL_SAME_BORROW_THREAD);
            }

            int autoReturnCount = returnQueue_onceOnBorrowThread.Count;
            SyncEvent_PlayerController_splitRotation autoReturn;
            while (returnQueue_onceOnBorrowThread.TryDequeue(out autoReturn) && autoReturnCount > 0)
            {
                Return(autoReturn);
                ++autoReturnCount;
            }

            var @event = pool.Borrow();
            
            @event.Explanation = explanation;
            @event.OccurredAtElapsedTicks = occurredAtElapsedTicks;
            @event.RelatedOwnerAuthorityId = relatedOwnerAuthorityId;
            @event.GONetId = gonetId;
			@event.CodeGenerationId = codeGenerationId;
            @event.SyncMemberIndex = syncMemberIndex;
			@event.valuePrevious = valuePrevious;
            @event.valueNew = valueNew;

            return @event;
		}

        public override void Return()
		{
			Return(this);
		}

        public static void Return(SyncEvent_PlayerController_splitRotation borrowed)
        {
            if (borrowThread == System.Threading.Thread.CurrentThread)
            {
                pool.Return(borrowed);
            }
            else
            {
                returnQueue_onceOnBorrowThread.Enqueue(borrowed);
            }
        }
    }

}

namespace GONet.Generation
{
	public partial class BobWad
	{
		static BobWad()
		{
			GONetParticipant_AutoMagicalSyncCompanion_Generated_Factory.theRealness = hahaThisIsTrulyTheRealness;
			GONetParticipant_AutoMagicalSyncCompanion_Generated_Factory.theRealness_quantizerSettings = hahaThisIsTrulyTheRealness_quantizerSettings;

			GONet_SyncEvent_ValueChangeProcessed_Generated_Factory.theRealness = hahaThisIsTrulyTheRealness_Events;
			GONet_SyncEvent_ValueChangeProcessed_Generated_Factory.theRealness_copy = hahaThisIsTrulyTheRealness_Events_Copy;

			GONet_SyncEvent_ValueChangeProcessed_Generated_Factory.allUniqueSyncEventTypes = new List<Type>()
			{
				typeof(BipedInputState),
				typeof(DroneInputsState),
				typeof(GONet.AutoMagicalSync_AllCurrentValues_Message),
				typeof(GONet.AutoMagicalSync_ValueChanges_Message),
				typeof(GONet.AutoMagicalSync_ValuesNowAtRest_Message),
				typeof(GONet.ClientRemotelyControlledGONetIdServerBatchAssignmentEvent),
				typeof(GONet.ClientRemotelyControlledGONetIdServerBatchRequestEvent),
				typeof(GONet.ClientStateChangedEvent),
				typeof(GONet.ClientTypeFlagsChangedEvent),
				typeof(GONet.DespawnGONetParticipantEvent),
				typeof(GONet.GONetParticipantDeserializeInitAllCompletedEvent),
				typeof(GONet.GONetParticipantDisabledEvent),
				typeof(GONet.GONetParticipantEnabledEvent),
				typeof(GONet.GONetParticipantStartedEvent),
				typeof(GONet.InstantiateGONetParticipantEvent),
				typeof(GONet.InternalOnlyMemoryPackComilationAssistanceForGenerated),
				typeof(GONet.OwnerAuthorityIdAssignmentEvent),
				typeof(GONet.PersistentEvents_Bundle),
				typeof(GONet.PersistentEvents_BundleChunk),
				typeof(GONet.PersistentRoutedRpcEvent),
				typeof(GONet.PersistentRpcEvent),
				typeof(GONet.RemoteClientStateChangedEvent),
				typeof(GONet.RequestMessage),
				typeof(GONet.ResponseMessage),
				typeof(GONet.RoutedRpcEvent),
				typeof(GONet.RpcDeliveryReportEvent),
				typeof(GONet.RpcEvent),
				typeof(GONet.RpcResponseEvent),
				typeof(GONet.SceneLoadCompleteEvent),
				typeof(GONet.SceneLoadEvent),
				typeof(GONet.SceneUnloadEvent),
				typeof(GONet.ServerSaysClientInitializationCompletion),
				typeof(GONet.SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId),
				typeof(GONet.SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount),
				typeof(GONet.SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles),
				typeof(GONet.SyncEvent_EntityStateSynchronizer_isCrouched),
				typeof(GONet.SyncEvent_EntityStateSynchronizer_isGrounded),
				typeof(GONet.SyncEvent_EntityStateSynchronizer_movementInput),
				typeof(GONet.SyncEvent_EntityStateSynchronizer_targetEntityId),
				typeof(GONet.SyncEvent_EntityStateSynchronizer_velocityVector),
				typeof(GONet.SyncEvent_GONetParticipant_GONetId),
				typeof(GONet.SyncEvent_GONetParticipant_IsPositionSyncd),
				typeof(GONet.SyncEvent_GONetParticipant_IsRotationSyncd),
				typeof(GONet.SyncEvent_GONetParticipant_OwnerAuthorityId),
				typeof(GONet.SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId),
				typeof(GONet.SyncEvent_PlayerController_splitRotation),
				typeof(GONet.SyncEvent_PlayerLocalContext__currentPlayerType),
				typeof(GONet.SyncEvent_PlayerLocalContext__isBipedInRagdollState),
				typeof(GONet.SyncEvent_PlayerLocalContext_currentTeamColor),
				typeof(GONet.SyncEvent_Time_ElapsedTicks_SetFromAuthority),
				typeof(GONet.SyncEvent_Transform_position),
				typeof(GONet.SyncEvent_Transform_rotation),
				typeof(GONet.ValueMonitoringSupport_BaselineExpiredEvent),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Boolean),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Byte),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Double),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Int16),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Int32),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Int64),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_SByte),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_Single),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_UInt16),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_UInt32),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_System_UInt64),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Quaternion),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector2),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector3),
				typeof(GONet.ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector4),
				typeof(InputPacket),
				typeof(InputStateEvent),
				typeof(LocalContextEntityEvent),
				typeof(MyEvent),
				typeof(SnapshotEvent),
				typeof(SnapshotState),
				typeof(GONet.SyncEvent_Time_ElapsedTicks_SetFromAuthority),
			};
		}

		static internal HashSet<GONet.Utils.QuantizerSettingsGroup> hahaThisIsTrulyTheRealness_quantizerSettings()
		{
			HashSet<GONet.Utils.QuantizerSettingsGroup> settings = new HashSet<GONet.Utils.QuantizerSettingsGroup>();

			var item_codeGenerationId1_single0_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId1_single0_singleMember0);

			var item_codeGenerationId1_single0_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId1_single0_singleMember1);

			var item_codeGenerationId1_single0_singleMember2 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId1_single0_singleMember2);

			var item_codeGenerationId1_single0_singleMember3 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId1_single0_singleMember3);

			var item_codeGenerationId1_single0_singleMember4 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId1_single0_singleMember4);

			var item_codeGenerationId1_single1_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId1_single1_singleMember0);

			var item_codeGenerationId1_single1_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-125f, 125f, 14, true);
			settings.Add(item_codeGenerationId1_single1_singleMember1);

			var item_codeGenerationId3_single0_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId3_single0_singleMember0);

			var item_codeGenerationId3_single0_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId3_single0_singleMember1);

			var item_codeGenerationId3_single0_singleMember2 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId3_single0_singleMember2);

			var item_codeGenerationId3_single0_singleMember3 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId3_single0_singleMember3);

			var item_codeGenerationId3_single0_singleMember4 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId3_single0_singleMember4);

			var item_codeGenerationId3_single1_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId3_single1_singleMember0);

			var item_codeGenerationId3_single1_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId3_single1_singleMember1);

			var item_codeGenerationId3_single2_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId3_single2_singleMember0);

			var item_codeGenerationId3_single2_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-125f, 125f, 14, true);
			settings.Add(item_codeGenerationId3_single2_singleMember1);

			var item_codeGenerationId5_single0_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId5_single0_singleMember0);

			var item_codeGenerationId5_single0_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId5_single0_singleMember1);

			var item_codeGenerationId5_single0_singleMember2 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId5_single0_singleMember2);

			var item_codeGenerationId5_single0_singleMember3 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId5_single0_singleMember3);

			var item_codeGenerationId5_single0_singleMember4 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId5_single0_singleMember4);

			var item_codeGenerationId5_single1_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-360f, 360f, 0, true);
			settings.Add(item_codeGenerationId5_single1_singleMember0);

			var item_codeGenerationId5_single1_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId5_single1_singleMember1);

			var item_codeGenerationId5_single1_singleMember2 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId5_single1_singleMember2);

			var item_codeGenerationId5_single1_singleMember3 = new GONet.Utils.QuantizerSettingsGroup(-1f, 1f, 0, true);
			settings.Add(item_codeGenerationId5_single1_singleMember3);

			var item_codeGenerationId5_single1_singleMember4 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId5_single1_singleMember4);

			var item_codeGenerationId5_single1_singleMember5 = new GONet.Utils.QuantizerSettingsGroup(-10f, 10f, 16, true);
			settings.Add(item_codeGenerationId5_single1_singleMember5);

			var item_codeGenerationId5_single2_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId5_single2_singleMember0);

			var item_codeGenerationId5_single2_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-125f, 125f, 14, true);
			settings.Add(item_codeGenerationId5_single2_singleMember1);

			var item_codeGenerationId4_single0_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId4_single0_singleMember0);

			var item_codeGenerationId4_single0_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId4_single0_singleMember1);

			var item_codeGenerationId4_single0_singleMember2 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId4_single0_singleMember2);

			var item_codeGenerationId4_single0_singleMember3 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId4_single0_singleMember3);

			var item_codeGenerationId4_single0_singleMember4 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId4_single0_singleMember4);

			var item_codeGenerationId4_single1_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId4_single1_singleMember0);

			var item_codeGenerationId4_single1_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId4_single1_singleMember1);

			var item_codeGenerationId4_single1_singleMember2 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId4_single1_singleMember2);

			var item_codeGenerationId4_single2_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId4_single2_singleMember0);

			var item_codeGenerationId4_single2_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-125f, 125f, 14, true);
			settings.Add(item_codeGenerationId4_single2_singleMember1);

			var item_codeGenerationId2_single0_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId2_single0_singleMember0);

			var item_codeGenerationId2_single0_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId2_single0_singleMember1);

			var item_codeGenerationId2_single0_singleMember2 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId2_single0_singleMember2);

			var item_codeGenerationId2_single0_singleMember3 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId2_single0_singleMember3);

			var item_codeGenerationId2_single0_singleMember4 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId2_single0_singleMember4);

			var item_codeGenerationId2_single1_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId2_single1_singleMember0);

			var item_codeGenerationId2_single2_singleMember0 = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);
			settings.Add(item_codeGenerationId2_single2_singleMember0);

			var item_codeGenerationId2_single2_singleMember1 = new GONet.Utils.QuantizerSettingsGroup(-125f, 125f, 14, true);
			settings.Add(item_codeGenerationId2_single2_singleMember1);

			return settings;
		}

		static internal GONetParticipant_AutoMagicalSyncCompanion_Generated hahaThisIsTrulyTheRealness(GONetParticipant gonetParticipant)
		{
			switch (gonetParticipant.CodeGenerationId)
			{
				case 1:
					return new GONetParticipant_AutoMagicalSyncCompanion_Generated_1(gonetParticipant);
				case 3:
					return new GONetParticipant_AutoMagicalSyncCompanion_Generated_3(gonetParticipant);
				case 5:
					return new GONetParticipant_AutoMagicalSyncCompanion_Generated_5(gonetParticipant);
				case 4:
					return new GONetParticipant_AutoMagicalSyncCompanion_Generated_4(gonetParticipant);
				case 2:
					return new GONetParticipant_AutoMagicalSyncCompanion_Generated_2(gonetParticipant);
			}

			return null;
		}

		internal static SyncEvent_ValueChangeProcessed hahaThisIsTrulyTheRealness_Events(SyncEvent_ValueChangeProcessedExplanation explanation, long elapsedTicks, ushort filterUsingOwnerAuthorityId, GONetParticipant_AutoMagicalSyncCompanion_Generated syncCompanion, byte syncMemberIndex)
        {
            switch (syncCompanion.gonetParticipant.CodeGenerationId)
            {
				case 1:
					{
						GONetParticipant_AutoMagicalSyncCompanion_Generated_1 companion = (GONetParticipant_AutoMagicalSyncCompanion_Generated_1)syncCompanion;
                        switch (syncMemberIndex)
                        {

                            case 0:
								{
									System.UInt32 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt32;
									}
									else
									{
																			valueNew = companion.GONetParticipant.GONetId;
																		}
									System.UInt32 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt32 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt32; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_GONetId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 1, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 1:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.GONetParticipant.IsPositionSyncd;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_IsPositionSyncd.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 1, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 2:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.GONetParticipant.IsRotationSyncd;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_IsRotationSyncd.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 1, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 3:
								{
									System.UInt16 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt16;
									}
									else
									{
																			valueNew = companion.GONetParticipant.OwnerAuthorityId;
																		}
									System.UInt16 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt16 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt16; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_OwnerAuthorityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 1, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 4:
								{
									System.UInt16 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt16;
									}
									else
									{
																			valueNew = companion.GONetParticipant.RemotelyControlledByAuthorityId;
																		}
									System.UInt16 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt16 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt16; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 1, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 5:
								{
									UnityEngine.Quaternion valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Quaternion;
									}
									else
									{
																			valueNew = companion.Transform.rotation;
																		}
									UnityEngine.Quaternion valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Quaternion : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Quaternion; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_Transform_rotation.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 1, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 6:
								{
									UnityEngine.Vector3 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Vector3;
									}
									else
									{
																			valueNew = companion.Transform.position;
																		}
									UnityEngine.Vector3 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Vector3 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Vector3; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_Transform_position.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 1, syncMemberIndex, valuePrevious, valueNew);
								}
						
						}
					}
					break;

				case 3:
					{
						GONetParticipant_AutoMagicalSyncCompanion_Generated_3 companion = (GONetParticipant_AutoMagicalSyncCompanion_Generated_3)syncCompanion;
                        switch (syncMemberIndex)
                        {

                            case 0:
								{
									System.UInt32 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt32;
									}
									else
									{
																			valueNew = companion.GONetParticipant.GONetId;
																		}
									System.UInt32 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt32 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt32; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_GONetId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 3, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 1:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.GONetParticipant.IsPositionSyncd;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_IsPositionSyncd.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 3, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 2:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.GONetParticipant.IsRotationSyncd;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_IsRotationSyncd.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 3, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 3:
								{
									System.UInt16 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt16;
									}
									else
									{
																			valueNew = companion.GONetParticipant.OwnerAuthorityId;
																		}
									System.UInt16 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt16 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt16; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_OwnerAuthorityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 3, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 4:
								{
									System.UInt16 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt16;
									}
									else
									{
																			valueNew = companion.GONetParticipant.RemotelyControlledByAuthorityId;
																		}
									System.UInt16 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt16 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt16; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 3, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 5:
								{
									System.UInt16 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt16;
									}
									else
									{
																			valueNew = companion.ClaimableObjectViaRpcs.ClaimedByAuthorityId;
																		}
									System.UInt16 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt16 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt16; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 3, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 6:
								{
									System.Int32 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Int32;
									}
									else
									{
																			valueNew = companion.ClaimableObjectViaRpcs.TotalClaimCount;
																		}
									System.Int32 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Int32 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Int32; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 3, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 7:
								{
									UnityEngine.Quaternion valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Quaternion;
									}
									else
									{
																			valueNew = companion.Transform.rotation;
																		}
									UnityEngine.Quaternion valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Quaternion : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Quaternion; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_Transform_rotation.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 3, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 8:
								{
									UnityEngine.Vector3 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Vector3;
									}
									else
									{
																			valueNew = companion.Transform.position;
																		}
									UnityEngine.Vector3 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Vector3 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Vector3; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_Transform_position.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 3, syncMemberIndex, valuePrevious, valueNew);
								}
						
						}
					}
					break;

				case 5:
					{
						GONetParticipant_AutoMagicalSyncCompanion_Generated_5 companion = (GONetParticipant_AutoMagicalSyncCompanion_Generated_5)syncCompanion;
                        switch (syncMemberIndex)
                        {

                            case 0:
								{
									System.UInt32 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt32;
									}
									else
									{
																			valueNew = companion.GONetParticipant.GONetId;
																		}
									System.UInt32 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt32 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt32; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_GONetId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 1:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.GONetParticipant.IsPositionSyncd;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_IsPositionSyncd.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 2:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.GONetParticipant.IsRotationSyncd;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_IsRotationSyncd.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 3:
								{
									System.UInt16 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt16;
									}
									else
									{
																			valueNew = companion.GONetParticipant.OwnerAuthorityId;
																		}
									System.UInt16 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt16 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt16; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_OwnerAuthorityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 4:
								{
									System.UInt16 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt16;
									}
									else
									{
																			valueNew = companion.GONetParticipant.RemotelyControlledByAuthorityId;
																		}
									System.UInt16 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt16 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt16; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 5:
								{
									UnityEngine.Vector3 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Vector3;
									}
									else
									{
																			valueNew = companion.EntityStateSynchronizer.cameraLookAtEulerAngles;
																		}
									UnityEngine.Vector3 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Vector3 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Vector3; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 6:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.EntityStateSynchronizer.isCrouched;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_EntityStateSynchronizer_isCrouched.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 7:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.EntityStateSynchronizer.isGrounded;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_EntityStateSynchronizer_isGrounded.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 8:
								{
									UnityEngine.Vector2 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Vector2;
									}
									else
									{
																			valueNew = companion.EntityStateSynchronizer.movementInput;
																		}
									UnityEngine.Vector2 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Vector2 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Vector2; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_EntityStateSynchronizer_movementInput.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 9:
								{
									System.UInt32 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt32;
									}
									else
									{
																			valueNew = companion.EntityStateSynchronizer.targetEntityId;
																		}
									System.UInt32 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt32 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt32; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_EntityStateSynchronizer_targetEntityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 10:
								{
									UnityEngine.Vector3 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Vector3;
									}
									else
									{
																			valueNew = companion.EntityStateSynchronizer.velocityVector;
																		}
									UnityEngine.Vector3 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Vector3 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Vector3; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_EntityStateSynchronizer_velocityVector.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 11:
								{
									UnityEngine.Quaternion valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Quaternion;
									}
									else
									{
																			valueNew = companion.Transform.rotation;
																		}
									UnityEngine.Quaternion valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Quaternion : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Quaternion; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_Transform_rotation.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 12:
								{
									UnityEngine.Vector3 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Vector3;
									}
									else
									{
																			valueNew = companion.Transform.position;
																		}
									UnityEngine.Vector3 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Vector3 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Vector3; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_Transform_position.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 5, syncMemberIndex, valuePrevious, valueNew);
								}
						
						}
					}
					break;

				case 4:
					{
						GONetParticipant_AutoMagicalSyncCompanion_Generated_4 companion = (GONetParticipant_AutoMagicalSyncCompanion_Generated_4)syncCompanion;
                        switch (syncMemberIndex)
                        {

                            case 0:
								{
									System.UInt32 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt32;
									}
									else
									{
																			valueNew = companion.GONetParticipant.GONetId;
																		}
									System.UInt32 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt32 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt32; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_GONetId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 4, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 1:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.GONetParticipant.IsPositionSyncd;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_IsPositionSyncd.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 4, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 2:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.GONetParticipant.IsRotationSyncd;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_IsRotationSyncd.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 4, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 3:
								{
									System.UInt16 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt16;
									}
									else
									{
																			valueNew = companion.GONetParticipant.OwnerAuthorityId;
																		}
									System.UInt16 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt16 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt16; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_OwnerAuthorityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 4, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 4:
								{
									System.UInt16 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt16;
									}
									else
									{
																			valueNew = companion.GONetParticipant.RemotelyControlledByAuthorityId;
																		}
									System.UInt16 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt16 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt16; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 4, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 5:
								{
									System.Byte valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Byte;
									}
									else
									{
																			valueNew = companion.PlayerLocalContext._currentPlayerType;
																		}
									System.Byte valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Byte : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Byte; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_PlayerLocalContext__currentPlayerType.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 4, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 6:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.PlayerLocalContext._isBipedInRagdollState;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_PlayerLocalContext__isBipedInRagdollState.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 4, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 7:
								{
									System.Byte valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Byte;
									}
									else
									{
																			valueNew = companion.PlayerLocalContext.currentTeamColor;
																		}
									System.Byte valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Byte : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Byte; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_PlayerLocalContext_currentTeamColor.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 4, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 8:
								{
									UnityEngine.Quaternion valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Quaternion;
									}
									else
									{
																			valueNew = companion.Transform.rotation;
																		}
									UnityEngine.Quaternion valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Quaternion : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Quaternion; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_Transform_rotation.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 4, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 9:
								{
									UnityEngine.Vector3 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Vector3;
									}
									else
									{
																			valueNew = companion.Transform.position;
																		}
									UnityEngine.Vector3 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Vector3 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Vector3; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_Transform_position.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 4, syncMemberIndex, valuePrevious, valueNew);
								}
						
						}
					}
					break;

				case 2:
					{
						GONetParticipant_AutoMagicalSyncCompanion_Generated_2 companion = (GONetParticipant_AutoMagicalSyncCompanion_Generated_2)syncCompanion;
                        switch (syncMemberIndex)
                        {

                            case 0:
								{
									System.UInt32 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt32;
									}
									else
									{
																			valueNew = companion.GONetParticipant.GONetId;
																		}
									System.UInt32 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt32 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt32; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_GONetId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 2, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 1:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.GONetParticipant.IsPositionSyncd;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_IsPositionSyncd.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 2, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 2:
								{
									System.Boolean valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_Boolean;
									}
									else
									{
																			valueNew = companion.GONetParticipant.IsRotationSyncd;
																		}
									System.Boolean valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_Boolean : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_Boolean; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_IsRotationSyncd.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 2, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 3:
								{
									System.UInt16 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt16;
									}
									else
									{
																			valueNew = companion.GONetParticipant.OwnerAuthorityId;
																		}
									System.UInt16 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt16 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt16; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_OwnerAuthorityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 2, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 4:
								{
									System.UInt16 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.System_UInt16;
									}
									else
									{
																			valueNew = companion.GONetParticipant.RemotelyControlledByAuthorityId;
																		}
									System.UInt16 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.System_UInt16 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.System_UInt16; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 2, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 5:
								{
									UnityEngine.Quaternion valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Quaternion;
									}
									else
									{
																			valueNew = companion.PlayerController.splitRotation;
																		}
									UnityEngine.Quaternion valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Quaternion : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Quaternion; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_PlayerController_splitRotation.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 2, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 6:
								{
									UnityEngine.Quaternion valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Quaternion;
									}
									else
									{
																			valueNew = companion.Transform.rotation;
																		}
									UnityEngine.Quaternion valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Quaternion : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Quaternion; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_Transform_rotation.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 2, syncMemberIndex, valuePrevious, valueNew);
								}
                            case 7:
								{
									UnityEngine.Vector3 valueNew;
									GONetSyncableValue valueNew_mostRecentChangeAtTime;
									if (explanation == SyncEvent_ValueChangeProcessedExplanation.InboundFromOther && companion.valuesChangesSupport[syncMemberIndex].TryGetMostRecentChangeAtTime(elapsedTicks, out valueNew_mostRecentChangeAtTime))
									{
										valueNew = valueNew_mostRecentChangeAtTime.UnityEngine_Vector3;
									}
									else
									{
																			valueNew = companion.Transform.position;
																		}
									UnityEngine.Vector3 valuePrevious = explanation == SyncEvent_ValueChangeProcessedExplanation.OutboundToOthers ? companion.valuesChangesSupport[syncMemberIndex].lastKnownValue_previous.UnityEngine_Vector3 : companion.valuesChangesSupport[syncMemberIndex].lastKnownValue.UnityEngine_Vector3; // because of order of operations and state of affairs at the time of publishing this event being different for the in/out direction, there is a different value to pull from that represents the previous value
									return SyncEvent_Transform_position.Borrow(explanation, elapsedTicks, filterUsingOwnerAuthorityId, syncCompanion.gonetParticipant.GONetId, 2, syncMemberIndex, valuePrevious, valueNew);
								}
						
						}
					}
					break;

            }

            return default;
        }

		internal static SyncEvent_ValueChangeProcessed hahaThisIsTrulyTheRealness_Events_Copy(SyncEvent_ValueChangeProcessed original)
		{
            switch (original.CodeGenerationId)
            {
				case 1:
					{
                        switch (original.SyncMemberIndex)
                        {

                            case 0:
								{
									SyncEvent_GONetParticipant_GONetId originalTyped = (SyncEvent_GONetParticipant_GONetId)original;
									return SyncEvent_GONetParticipant_GONetId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 1:
								{
									SyncEvent_GONetParticipant_IsPositionSyncd originalTyped = (SyncEvent_GONetParticipant_IsPositionSyncd)original;
									return SyncEvent_GONetParticipant_IsPositionSyncd.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 2:
								{
									SyncEvent_GONetParticipant_IsRotationSyncd originalTyped = (SyncEvent_GONetParticipant_IsRotationSyncd)original;
									return SyncEvent_GONetParticipant_IsRotationSyncd.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 3:
								{
									SyncEvent_GONetParticipant_OwnerAuthorityId originalTyped = (SyncEvent_GONetParticipant_OwnerAuthorityId)original;
									return SyncEvent_GONetParticipant_OwnerAuthorityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 4:
								{
									SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId originalTyped = (SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId)original;
									return SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 5:
								{
									SyncEvent_Transform_rotation originalTyped = (SyncEvent_Transform_rotation)original;
									return SyncEvent_Transform_rotation.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 6:
								{
									SyncEvent_Transform_position originalTyped = (SyncEvent_Transform_position)original;
									return SyncEvent_Transform_position.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
						
						}
					}
					break;

				case 3:
					{
                        switch (original.SyncMemberIndex)
                        {

                            case 0:
								{
									SyncEvent_GONetParticipant_GONetId originalTyped = (SyncEvent_GONetParticipant_GONetId)original;
									return SyncEvent_GONetParticipant_GONetId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 1:
								{
									SyncEvent_GONetParticipant_IsPositionSyncd originalTyped = (SyncEvent_GONetParticipant_IsPositionSyncd)original;
									return SyncEvent_GONetParticipant_IsPositionSyncd.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 2:
								{
									SyncEvent_GONetParticipant_IsRotationSyncd originalTyped = (SyncEvent_GONetParticipant_IsRotationSyncd)original;
									return SyncEvent_GONetParticipant_IsRotationSyncd.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 3:
								{
									SyncEvent_GONetParticipant_OwnerAuthorityId originalTyped = (SyncEvent_GONetParticipant_OwnerAuthorityId)original;
									return SyncEvent_GONetParticipant_OwnerAuthorityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 4:
								{
									SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId originalTyped = (SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId)original;
									return SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 5:
								{
									SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId originalTyped = (SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId)original;
									return SyncEvent_ClaimableObjectViaRpcs_ClaimedByAuthorityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 6:
								{
									SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount originalTyped = (SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount)original;
									return SyncEvent_ClaimableObjectViaRpcs_TotalClaimCount.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 7:
								{
									SyncEvent_Transform_rotation originalTyped = (SyncEvent_Transform_rotation)original;
									return SyncEvent_Transform_rotation.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 8:
								{
									SyncEvent_Transform_position originalTyped = (SyncEvent_Transform_position)original;
									return SyncEvent_Transform_position.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
						
						}
					}
					break;

				case 5:
					{
                        switch (original.SyncMemberIndex)
                        {

                            case 0:
								{
									SyncEvent_GONetParticipant_GONetId originalTyped = (SyncEvent_GONetParticipant_GONetId)original;
									return SyncEvent_GONetParticipant_GONetId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 1:
								{
									SyncEvent_GONetParticipant_IsPositionSyncd originalTyped = (SyncEvent_GONetParticipant_IsPositionSyncd)original;
									return SyncEvent_GONetParticipant_IsPositionSyncd.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 2:
								{
									SyncEvent_GONetParticipant_IsRotationSyncd originalTyped = (SyncEvent_GONetParticipant_IsRotationSyncd)original;
									return SyncEvent_GONetParticipant_IsRotationSyncd.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 3:
								{
									SyncEvent_GONetParticipant_OwnerAuthorityId originalTyped = (SyncEvent_GONetParticipant_OwnerAuthorityId)original;
									return SyncEvent_GONetParticipant_OwnerAuthorityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 4:
								{
									SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId originalTyped = (SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId)original;
									return SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 5:
								{
									SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles originalTyped = (SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles)original;
									return SyncEvent_EntityStateSynchronizer_cameraLookAtEulerAngles.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 6:
								{
									SyncEvent_EntityStateSynchronizer_isCrouched originalTyped = (SyncEvent_EntityStateSynchronizer_isCrouched)original;
									return SyncEvent_EntityStateSynchronizer_isCrouched.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 7:
								{
									SyncEvent_EntityStateSynchronizer_isGrounded originalTyped = (SyncEvent_EntityStateSynchronizer_isGrounded)original;
									return SyncEvent_EntityStateSynchronizer_isGrounded.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 8:
								{
									SyncEvent_EntityStateSynchronizer_movementInput originalTyped = (SyncEvent_EntityStateSynchronizer_movementInput)original;
									return SyncEvent_EntityStateSynchronizer_movementInput.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 9:
								{
									SyncEvent_EntityStateSynchronizer_targetEntityId originalTyped = (SyncEvent_EntityStateSynchronizer_targetEntityId)original;
									return SyncEvent_EntityStateSynchronizer_targetEntityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 10:
								{
									SyncEvent_EntityStateSynchronizer_velocityVector originalTyped = (SyncEvent_EntityStateSynchronizer_velocityVector)original;
									return SyncEvent_EntityStateSynchronizer_velocityVector.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 11:
								{
									SyncEvent_Transform_rotation originalTyped = (SyncEvent_Transform_rotation)original;
									return SyncEvent_Transform_rotation.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 12:
								{
									SyncEvent_Transform_position originalTyped = (SyncEvent_Transform_position)original;
									return SyncEvent_Transform_position.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
						
						}
					}
					break;

				case 4:
					{
                        switch (original.SyncMemberIndex)
                        {

                            case 0:
								{
									SyncEvent_GONetParticipant_GONetId originalTyped = (SyncEvent_GONetParticipant_GONetId)original;
									return SyncEvent_GONetParticipant_GONetId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 1:
								{
									SyncEvent_GONetParticipant_IsPositionSyncd originalTyped = (SyncEvent_GONetParticipant_IsPositionSyncd)original;
									return SyncEvent_GONetParticipant_IsPositionSyncd.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 2:
								{
									SyncEvent_GONetParticipant_IsRotationSyncd originalTyped = (SyncEvent_GONetParticipant_IsRotationSyncd)original;
									return SyncEvent_GONetParticipant_IsRotationSyncd.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 3:
								{
									SyncEvent_GONetParticipant_OwnerAuthorityId originalTyped = (SyncEvent_GONetParticipant_OwnerAuthorityId)original;
									return SyncEvent_GONetParticipant_OwnerAuthorityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 4:
								{
									SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId originalTyped = (SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId)original;
									return SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 5:
								{
									SyncEvent_PlayerLocalContext__currentPlayerType originalTyped = (SyncEvent_PlayerLocalContext__currentPlayerType)original;
									return SyncEvent_PlayerLocalContext__currentPlayerType.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 6:
								{
									SyncEvent_PlayerLocalContext__isBipedInRagdollState originalTyped = (SyncEvent_PlayerLocalContext__isBipedInRagdollState)original;
									return SyncEvent_PlayerLocalContext__isBipedInRagdollState.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 7:
								{
									SyncEvent_PlayerLocalContext_currentTeamColor originalTyped = (SyncEvent_PlayerLocalContext_currentTeamColor)original;
									return SyncEvent_PlayerLocalContext_currentTeamColor.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 8:
								{
									SyncEvent_Transform_rotation originalTyped = (SyncEvent_Transform_rotation)original;
									return SyncEvent_Transform_rotation.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 9:
								{
									SyncEvent_Transform_position originalTyped = (SyncEvent_Transform_position)original;
									return SyncEvent_Transform_position.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
						
						}
					}
					break;

				case 2:
					{
                        switch (original.SyncMemberIndex)
                        {

                            case 0:
								{
									SyncEvent_GONetParticipant_GONetId originalTyped = (SyncEvent_GONetParticipant_GONetId)original;
									return SyncEvent_GONetParticipant_GONetId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 1:
								{
									SyncEvent_GONetParticipant_IsPositionSyncd originalTyped = (SyncEvent_GONetParticipant_IsPositionSyncd)original;
									return SyncEvent_GONetParticipant_IsPositionSyncd.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 2:
								{
									SyncEvent_GONetParticipant_IsRotationSyncd originalTyped = (SyncEvent_GONetParticipant_IsRotationSyncd)original;
									return SyncEvent_GONetParticipant_IsRotationSyncd.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 3:
								{
									SyncEvent_GONetParticipant_OwnerAuthorityId originalTyped = (SyncEvent_GONetParticipant_OwnerAuthorityId)original;
									return SyncEvent_GONetParticipant_OwnerAuthorityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 4:
								{
									SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId originalTyped = (SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId)original;
									return SyncEvent_GONetParticipant_RemotelyControlledByAuthorityId.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 5:
								{
									SyncEvent_PlayerController_splitRotation originalTyped = (SyncEvent_PlayerController_splitRotation)original;
									return SyncEvent_PlayerController_splitRotation.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 6:
								{
									SyncEvent_Transform_rotation originalTyped = (SyncEvent_Transform_rotation)original;
									return SyncEvent_Transform_rotation.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
                            case 7:
								{
									SyncEvent_Transform_position originalTyped = (SyncEvent_Transform_position)original;
									return SyncEvent_Transform_position.Borrow(original.Explanation, original.OccurredAtElapsedTicks, original.RelatedOwnerAuthorityId, original.GONetId, original.CodeGenerationId, original.SyncMemberIndex, originalTyped.valuePrevious, originalTyped.valueNew);
								}
						
						}
					}
					break;

			}

			return default;
		}
	}
}