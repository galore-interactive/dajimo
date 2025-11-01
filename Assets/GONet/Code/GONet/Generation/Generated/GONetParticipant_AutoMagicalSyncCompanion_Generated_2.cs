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
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using GONet;

namespace GONet.Generation
{
	internal sealed class GONetParticipant_AutoMagicalSyncCompanion_Generated_2 : GONetParticipant_AutoMagicalSyncCompanion_Generated
    {
		private GONet.GONetParticipant _GONetParticipant;
		internal GONet.GONetParticipant GONetParticipant
		{
			get
			{
				if ((object)_GONetParticipant == null)
				{
					_GONetParticipant = gonetParticipant.GetComponent<GONet.GONetParticipant>();
				}
				return _GONetParticipant;
			}
		}

		private PlayerController _PlayerController;
		internal PlayerController PlayerController
		{
			get
			{
				if ((object)_PlayerController == null)
				{
					_PlayerController = gonetParticipant.GetComponent<PlayerController>();
				}
				return _PlayerController;
			}
		}

		private UnityEngine.Transform _Transform;
		internal UnityEngine.Transform Transform
		{
			get
			{
				if ((object)_Transform == null)
				{
					_Transform = gonetParticipant.GetComponent<UnityEngine.Transform>();
				}
				return _Transform;
			}
		}

		/// <summary>
		/// PHYSICS TIMING: Cached physics check for Transform.position/rotation time source selection.
		/// Initialized once in constructor based on gonetParticipant.IsRigidBodyOwnerOnlyControlled.
		/// NOTE: Changing IsRigidBodyOwnerOnlyControlled at runtime will NOT update this cached value!
		/// This is an optimization to avoid redundant checks in constructor and UpdateLastKnownValues hot paths.
		/// </summary>
		private readonly bool shouldSourceFromRigidbody;

        internal override byte CodeGenerationId => 2;

        internal GONetParticipant_AutoMagicalSyncCompanion_Generated_2(GONetParticipant gonetParticipant) : base(gonetParticipant)
		{
			valuesCount = 8;
			
			cachedCustomSerializers = cachedCustomSerializersArrayPool.Borrow((int)valuesCount);
			cachedValueSerializers = cachedCustomSerializersArrayPool.Borrow((int)valuesCount);
			cachedVelocitySerializers = cachedCustomSerializersArrayPool.Borrow((int)valuesCount);
			cachedCustomValueBlendings = cachedCustomValueBlendingsArrayPool.Borrow((int)valuesCount);
			cachedCustomVelocityBlendings = cachedCustomVelocityBlendingsArrayPool.Borrow((int)valuesCount);

			// Velocity-augmented sync: Allocate syncCounter for per-value velocity frequency tracking
			syncCounter = syncCounterArrayPool.Borrow((int)valuesCount);
			Array.Clear(syncCounter, 0, syncCounter.Length); // Start at 0 for all values
		    
			lastKnownValueChangesSinceLastCheck = lastKnownValuesChangedArrayPool.Borrow((int)valuesCount);
			Array.Clear(lastKnownValueChangesSinceLastCheck, 0, lastKnownValueChangesSinceLastCheck.Length);

			lastKnownValueAtRestBits = lastKnownValueAtRestBitsArrayPool.Borrow((int)valuesCount);
			lastKnownValueChangedAtElapsedTicks = lastKnownValueChangedAtElapsedTicksArrayPool.Borrow((int)valuesCount);
			for (int i = 0; i < (int)valuesCount; ++i)
			{
				lastKnownValueAtRestBits[i] = LAST_KNOWN_VALUE_IS_AT_REST_ALREADY_BROADCASTED; // when things start consider things at rest and alreayd broadcast as to avoid trying to broadcast at rest too early in the beginning
				lastKnownValueChangedAtElapsedTicks[i] = long.MaxValue; // want to start high so the subtraction from actual game time later on does not yield a high value on first times before set with a proper/real value of last change...which would cause an unwanted false positive
			}
			
            doesBaselineValueNeedAdjusting = doesBaselineValueNeedAdjustingArrayPool.Borrow((int)valuesCount);
            Array.Clear(doesBaselineValueNeedAdjusting, 0, doesBaselineValueNeedAdjusting.Length);

			valuesChangesSupport = valuesChangesSupportArrayPool.Borrow((int)valuesCount);
			
			// PHYSICS TIMING: Initialize class field for physics timing (used in constructor and UpdateLastKnownValues)
			// CRITICAL: myRigidBody is not set until GONetParticipant.Start(), so we need to do the lookup ourselves
			if (gonetParticipant.myRigidBody == null)
			{
				gonetParticipant.myRigidBody = gonetParticipant.GetComponent<UnityEngine.Rigidbody>();
			}
			shouldSourceFromRigidbody = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;

			var support0 = valuesChangesSupport[0] = valueChangeSupportArrayPool.Borrow();
            support0.baselineValue_current.System_UInt32 = GONetParticipant.GONetId; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support0.lastKnownValue.System_UInt32 = GONetParticipant.GONetId; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support0.lastKnownValue_previous.System_UInt32 = GONetParticipant.GONetId; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support0.valueLimitEncountered_min.System_UInt32 = GONetParticipant.GONetId; 
			support0.valueLimitEncountered_max.System_UInt32 = GONetParticipant.GONetId; 
			support0.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support0.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support0.syncCompanion = this;
			support0.memberName = "GONetId";
			support0.index = 0;
			support0.syncAttribute_MustRunOnUnityMainThread = true;
			support0.syncAttribute_ProcessingPriority = 0;
			support0.syncAttribute_ProcessingPriority_GONetInternalOverride = 2147483647;
			support0.syncAttribute_SyncChangesEverySeconds = 0f;
			support0.syncAttribute_Reliability = AutoMagicalSyncReliability.Reliable;
			support0.syncAttribute_ShouldBlendBetweenValuesReceived = false;
			support0.syncAttribute_PhysicsUpdateInterval = 1;
			support0.syncAttribute_EnablePhysicsSnapping = false;
			support0.isVelocityEligible = false;
			support0.syncAttribute_VelocityQuantizeLowerBound = -20.000000f;
			support0.syncAttribute_VelocityQuantizeUpperBound = 20.000000f;
			support0.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support0_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support0_effectiveSyncInterval = support0_isPhysicsObject && support0.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support0.syncAttribute_PhysicsUpdateInterval
				: support0.syncAttribute_SyncChangesEverySeconds;
			support0.velocityQuantizeLowerBound_PerSyncInterval = support0.syncAttribute_VelocityQuantizeLowerBound * support0_effectiveSyncInterval;
			support0.velocityQuantizeUpperBound_PerSyncInterval = support0.syncAttribute_VelocityQuantizeUpperBound * support0_effectiveSyncInterval;
			support0.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support0.codeGenerationMemberType = GONetSyncableValueTypes.System_UInt32;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((2, 0), out support0.syncAttribute_ShouldSkipSync);
			support0.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);

			cachedCustomSerializers[0] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.GONetParticipant.GONetId_InitialAssignment_CustomSerializer>(0, -1.701412E+38f, 1.701412E+38f);
			cachedValueSerializers[0] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.GONetParticipant.GONetId_InitialAssignment_CustomSerializer>(0, -1.701412E+38f, 1.701412E+38f);
			cachedVelocitySerializers[0] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.GONetParticipant.GONetId_InitialAssignment_CustomSerializer>(18, -20.000000f, 20.000000f);


			var support1 = valuesChangesSupport[1] = valueChangeSupportArrayPool.Borrow();
            support1.baselineValue_current.System_Boolean = GONetParticipant.IsPositionSyncd; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support1.lastKnownValue.System_Boolean = GONetParticipant.IsPositionSyncd; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support1.lastKnownValue_previous.System_Boolean = GONetParticipant.IsPositionSyncd; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support1.valueLimitEncountered_min.System_Boolean = GONetParticipant.IsPositionSyncd; 
			support1.valueLimitEncountered_max.System_Boolean = GONetParticipant.IsPositionSyncd; 
			support1.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support1.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support1.syncCompanion = this;
			support1.memberName = "IsPositionSyncd";
			support1.index = 1;
			support1.syncAttribute_MustRunOnUnityMainThread = true;
			support1.syncAttribute_ProcessingPriority = 0;
			support1.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support1.syncAttribute_SyncChangesEverySeconds = 0.04166667f;
			support1.syncAttribute_Reliability = AutoMagicalSyncReliability.Unreliable;
			support1.syncAttribute_ShouldBlendBetweenValuesReceived = true;
			support1.syncAttribute_PhysicsUpdateInterval = 1;
			support1.syncAttribute_EnablePhysicsSnapping = false;
			support1.isVelocityEligible = false;
			support1.syncAttribute_VelocityQuantizeLowerBound = -20.000000f;
			support1.syncAttribute_VelocityQuantizeUpperBound = 20.000000f;
			support1.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support1_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support1_effectiveSyncInterval = support1_isPhysicsObject && support1.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support1.syncAttribute_PhysicsUpdateInterval
				: support1.syncAttribute_SyncChangesEverySeconds;
			support1.velocityQuantizeLowerBound_PerSyncInterval = support1.syncAttribute_VelocityQuantizeLowerBound * support1_effectiveSyncInterval;
			support1.velocityQuantizeUpperBound_PerSyncInterval = support1.syncAttribute_VelocityQuantizeUpperBound * support1_effectiveSyncInterval;
			support1.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support1.codeGenerationMemberType = GONetSyncableValueTypes.System_Boolean;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((2, 1), out support1.syncAttribute_ShouldSkipSync);
			support1.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);

            int support1_mostRecentChanges_calcdSize = support1.syncAttribute_SyncChangesEverySeconds != 0 ? (int)((GONetMain.valueBlendingBufferLeadSeconds / support1.syncAttribute_SyncChangesEverySeconds) * 2.5f) : 0;
            support1.mostRecentChanges_capacitySize = Math.Max(support1_mostRecentChanges_calcdSize, GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.MOST_RECENT_CHANGEs_SIZE_MINIMUM);
			support1.mostRecentChanges = GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.mostRecentChangesPool.Borrow(support1.mostRecentChanges_capacitySize);

			var support2 = valuesChangesSupport[2] = valueChangeSupportArrayPool.Borrow();
            support2.baselineValue_current.System_Boolean = GONetParticipant.IsRotationSyncd; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support2.lastKnownValue.System_Boolean = GONetParticipant.IsRotationSyncd; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support2.lastKnownValue_previous.System_Boolean = GONetParticipant.IsRotationSyncd; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support2.valueLimitEncountered_min.System_Boolean = GONetParticipant.IsRotationSyncd; 
			support2.valueLimitEncountered_max.System_Boolean = GONetParticipant.IsRotationSyncd; 
			support2.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support2.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support2.syncCompanion = this;
			support2.memberName = "IsRotationSyncd";
			support2.index = 2;
			support2.syncAttribute_MustRunOnUnityMainThread = true;
			support2.syncAttribute_ProcessingPriority = 0;
			support2.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support2.syncAttribute_SyncChangesEverySeconds = 0.04166667f;
			support2.syncAttribute_Reliability = AutoMagicalSyncReliability.Unreliable;
			support2.syncAttribute_ShouldBlendBetweenValuesReceived = true;
			support2.syncAttribute_PhysicsUpdateInterval = 1;
			support2.syncAttribute_EnablePhysicsSnapping = false;
			support2.isVelocityEligible = false;
			support2.syncAttribute_VelocityQuantizeLowerBound = -20.000000f;
			support2.syncAttribute_VelocityQuantizeUpperBound = 20.000000f;
			support2.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support2_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support2_effectiveSyncInterval = support2_isPhysicsObject && support2.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support2.syncAttribute_PhysicsUpdateInterval
				: support2.syncAttribute_SyncChangesEverySeconds;
			support2.velocityQuantizeLowerBound_PerSyncInterval = support2.syncAttribute_VelocityQuantizeLowerBound * support2_effectiveSyncInterval;
			support2.velocityQuantizeUpperBound_PerSyncInterval = support2.syncAttribute_VelocityQuantizeUpperBound * support2_effectiveSyncInterval;
			support2.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support2.codeGenerationMemberType = GONetSyncableValueTypes.System_Boolean;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((2, 2), out support2.syncAttribute_ShouldSkipSync);
			support2.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);

            int support2_mostRecentChanges_calcdSize = support2.syncAttribute_SyncChangesEverySeconds != 0 ? (int)((GONetMain.valueBlendingBufferLeadSeconds / support2.syncAttribute_SyncChangesEverySeconds) * 2.5f) : 0;
            support2.mostRecentChanges_capacitySize = Math.Max(support2_mostRecentChanges_calcdSize, GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.MOST_RECENT_CHANGEs_SIZE_MINIMUM);
			support2.mostRecentChanges = GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.mostRecentChangesPool.Borrow(support2.mostRecentChanges_capacitySize);

			var support3 = valuesChangesSupport[3] = valueChangeSupportArrayPool.Borrow();
            support3.baselineValue_current.System_UInt16 = GONetParticipant.OwnerAuthorityId; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support3.lastKnownValue.System_UInt16 = GONetParticipant.OwnerAuthorityId; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support3.lastKnownValue_previous.System_UInt16 = GONetParticipant.OwnerAuthorityId; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support3.valueLimitEncountered_min.System_UInt16 = GONetParticipant.OwnerAuthorityId; 
			support3.valueLimitEncountered_max.System_UInt16 = GONetParticipant.OwnerAuthorityId; 
			support3.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support3.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support3.syncCompanion = this;
			support3.memberName = "OwnerAuthorityId";
			support3.index = 3;
			support3.syncAttribute_MustRunOnUnityMainThread = true;
			support3.syncAttribute_ProcessingPriority = 0;
			support3.syncAttribute_ProcessingPriority_GONetInternalOverride = 2147483646;
			support3.syncAttribute_SyncChangesEverySeconds = 0f;
			support3.syncAttribute_Reliability = AutoMagicalSyncReliability.Reliable;
			support3.syncAttribute_ShouldBlendBetweenValuesReceived = false;
			support3.syncAttribute_PhysicsUpdateInterval = 1;
			support3.syncAttribute_EnablePhysicsSnapping = false;
			support3.isVelocityEligible = false;
			support3.syncAttribute_VelocityQuantizeLowerBound = -20.000000f;
			support3.syncAttribute_VelocityQuantizeUpperBound = 20.000000f;
			support3.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support3_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support3_effectiveSyncInterval = support3_isPhysicsObject && support3.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support3.syncAttribute_PhysicsUpdateInterval
				: support3.syncAttribute_SyncChangesEverySeconds;
			support3.velocityQuantizeLowerBound_PerSyncInterval = support3.syncAttribute_VelocityQuantizeLowerBound * support3_effectiveSyncInterval;
			support3.velocityQuantizeUpperBound_PerSyncInterval = support3.syncAttribute_VelocityQuantizeUpperBound * support3_effectiveSyncInterval;
			support3.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support3.codeGenerationMemberType = GONetSyncableValueTypes.System_UInt16;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((2, 3), out support3.syncAttribute_ShouldSkipSync);
			support3.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);


			var support4 = valuesChangesSupport[4] = valueChangeSupportArrayPool.Borrow();
            support4.baselineValue_current.System_UInt16 = GONetParticipant.RemotelyControlledByAuthorityId; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support4.lastKnownValue.System_UInt16 = GONetParticipant.RemotelyControlledByAuthorityId; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support4.lastKnownValue_previous.System_UInt16 = GONetParticipant.RemotelyControlledByAuthorityId; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support4.valueLimitEncountered_min.System_UInt16 = GONetParticipant.RemotelyControlledByAuthorityId; 
			support4.valueLimitEncountered_max.System_UInt16 = GONetParticipant.RemotelyControlledByAuthorityId; 
			support4.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support4.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support4.syncCompanion = this;
			support4.memberName = "RemotelyControlledByAuthorityId";
			support4.index = 4;
			support4.syncAttribute_MustRunOnUnityMainThread = true;
			support4.syncAttribute_ProcessingPriority = 0;
			support4.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support4.syncAttribute_SyncChangesEverySeconds = 0f;
			support4.syncAttribute_Reliability = AutoMagicalSyncReliability.Reliable;
			support4.syncAttribute_ShouldBlendBetweenValuesReceived = false;
			support4.syncAttribute_PhysicsUpdateInterval = 1;
			support4.syncAttribute_EnablePhysicsSnapping = false;
			support4.isVelocityEligible = false;
			support4.syncAttribute_VelocityQuantizeLowerBound = -20.000000f;
			support4.syncAttribute_VelocityQuantizeUpperBound = 20.000000f;
			support4.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support4_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support4_effectiveSyncInterval = support4_isPhysicsObject && support4.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support4.syncAttribute_PhysicsUpdateInterval
				: support4.syncAttribute_SyncChangesEverySeconds;
			support4.velocityQuantizeLowerBound_PerSyncInterval = support4.syncAttribute_VelocityQuantizeLowerBound * support4_effectiveSyncInterval;
			support4.velocityQuantizeUpperBound_PerSyncInterval = support4.syncAttribute_VelocityQuantizeUpperBound * support4_effectiveSyncInterval;
			support4.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support4.codeGenerationMemberType = GONetSyncableValueTypes.System_UInt16;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((2, 4), out support4.syncAttribute_ShouldSkipSync);
			support4.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);


			var support5 = valuesChangesSupport[5] = valueChangeSupportArrayPool.Borrow();
            support5.baselineValue_current.UnityEngine_Quaternion = PlayerController.splitRotation; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support5.lastKnownValue.UnityEngine_Quaternion = PlayerController.splitRotation; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support5.lastKnownValue_previous.UnityEngine_Quaternion = PlayerController.splitRotation; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support5.valueLimitEncountered_min.UnityEngine_Quaternion = PlayerController.splitRotation; 
			support5.valueLimitEncountered_max.UnityEngine_Quaternion = PlayerController.splitRotation; 
			support5.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support5.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support5.syncCompanion = this;
			support5.memberName = "splitRotation";
			support5.index = 5;
			support5.syncAttribute_MustRunOnUnityMainThread = true;
			support5.syncAttribute_ProcessingPriority = 0;
			support5.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support5.syncAttribute_SyncChangesEverySeconds = 0.04166667f;
			support5.syncAttribute_Reliability = AutoMagicalSyncReliability.Unreliable;
			support5.syncAttribute_ShouldBlendBetweenValuesReceived = true;
			support5.syncAttribute_PhysicsUpdateInterval = 1;
			support5.syncAttribute_EnablePhysicsSnapping = false;
			support5.isVelocityEligible = true;
			support5.syncAttribute_VelocityQuantizeLowerBound = -0.133614f;
			support5.syncAttribute_VelocityQuantizeUpperBound = 0.133614f;
			support5.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support5_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support5_effectiveSyncInterval = support5_isPhysicsObject && support5.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support5.syncAttribute_PhysicsUpdateInterval
				: support5.syncAttribute_SyncChangesEverySeconds;
			support5.velocityQuantizeLowerBound_PerSyncInterval = support5.syncAttribute_VelocityQuantizeLowerBound * support5_effectiveSyncInterval;
			support5.velocityQuantizeUpperBound_PerSyncInterval = support5.syncAttribute_VelocityQuantizeUpperBound * support5_effectiveSyncInterval;
			support5.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support5.codeGenerationMemberType = GONetSyncableValueTypes.UnityEngine_Quaternion;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((2, 5), out support5.syncAttribute_ShouldSkipSync);
			support5.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);

			cachedCustomSerializers[5] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.QuaternionSerializer>(0, -1.701412E+38f, 1.701412E+38f);
			cachedValueSerializers[5] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.QuaternionSerializer>(0, -1.701412E+38f, 1.701412E+38f);
			cachedVelocitySerializers[5] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(9, -0.133614f, 0.133614f);

			cachedCustomValueBlendings[5] = GONetAutoMagicalSyncAttribute.GetCustomValueBlending<GONet.PluginAPI.GONetValueBlending_Quaternion_ExtrapolateWithLowPassSmoothingFilter>();
			cachedCustomVelocityBlendings[5] = GONet.Utils.ValueBlendUtils.GetDefaultVelocityBlending(support5.lastKnownValue.GONetSyncType);
            int support5_mostRecentChanges_calcdSize = support5.syncAttribute_SyncChangesEverySeconds != 0 ? (int)((GONetMain.valueBlendingBufferLeadSeconds / support5.syncAttribute_SyncChangesEverySeconds) * 2.5f) : 0;
            support5.mostRecentChanges_capacitySize = Math.Max(support5_mostRecentChanges_calcdSize, GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.MOST_RECENT_CHANGEs_SIZE_MINIMUM);
			support5.mostRecentChanges = GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.mostRecentChangesPool.Borrow(support5.mostRecentChanges_capacitySize);

			var support6 = valuesChangesSupport[6] = valueChangeSupportArrayPool.Borrow();
            support6.baselineValue_current.UnityEngine_Quaternion = Transform.rotation; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support6.lastKnownValue.UnityEngine_Quaternion = Transform.rotation; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support6.lastKnownValue_previous.UnityEngine_Quaternion = Transform.rotation; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support6.valueLimitEncountered_min.UnityEngine_Quaternion = Transform.rotation; 
			support6.valueLimitEncountered_max.UnityEngine_Quaternion = Transform.rotation; 
			support6.lastKnownValue_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support6.lastKnownValue_previous_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support6.syncCompanion = this;
			support6.memberName = "rotation";
			support6.index = 6;
			support6.syncAttribute_MustRunOnUnityMainThread = true;
			support6.syncAttribute_ProcessingPriority = 0;
			support6.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support6.syncAttribute_SyncChangesEverySeconds = 0.04166667f;
			support6.syncAttribute_Reliability = AutoMagicalSyncReliability.Unreliable;
			support6.syncAttribute_ShouldBlendBetweenValuesReceived = true;
			support6.syncAttribute_PhysicsUpdateInterval = 1;
			support6.syncAttribute_EnablePhysicsSnapping = false;
			support6.isVelocityEligible = true;
			support6.syncAttribute_VelocityQuantizeLowerBound = -0.133614f;
			support6.syncAttribute_VelocityQuantizeUpperBound = 0.133614f;
			support6.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support6_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support6_effectiveSyncInterval = support6_isPhysicsObject && support6.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support6.syncAttribute_PhysicsUpdateInterval
				: support6.syncAttribute_SyncChangesEverySeconds;
			support6.velocityQuantizeLowerBound_PerSyncInterval = support6.syncAttribute_VelocityQuantizeLowerBound * support6_effectiveSyncInterval;
			support6.velocityQuantizeUpperBound_PerSyncInterval = support6.syncAttribute_VelocityQuantizeUpperBound * support6_effectiveSyncInterval;
			support6.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support6.codeGenerationMemberType = GONetSyncableValueTypes.UnityEngine_Quaternion;
			support6.syncAttribute_ShouldSkipSync = GONetMain.IsRotationNotSyncd;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((2, 6), out support6.syncAttribute_ShouldSkipSync);
			support6.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);

			cachedCustomSerializers[6] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.QuaternionSerializer>(0, -1.701412E+38f, 1.701412E+38f);
			cachedValueSerializers[6] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.QuaternionSerializer>(0, -1.701412E+38f, 1.701412E+38f);
			cachedVelocitySerializers[6] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(9, -0.133614f, 0.133614f);

			cachedCustomValueBlendings[6] = GONetAutoMagicalSyncAttribute.GetCustomValueBlending<GONet.PluginAPI.GONetValueBlending_Quaternion_ExtrapolateWithLowPassSmoothingFilter>();
			cachedCustomVelocityBlendings[6] = GONet.Utils.ValueBlendUtils.GetDefaultVelocityBlending(support6.lastKnownValue.GONetSyncType);
            int support6_mostRecentChanges_calcdSize = support6.syncAttribute_SyncChangesEverySeconds != 0 ? (int)((GONetMain.valueBlendingBufferLeadSeconds / support6.syncAttribute_SyncChangesEverySeconds) * 2.5f) : 0;
            support6.mostRecentChanges_capacitySize = Math.Max(support6_mostRecentChanges_calcdSize, GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.MOST_RECENT_CHANGEs_SIZE_MINIMUM);
			support6.mostRecentChanges = GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.mostRecentChangesPool.Borrow(support6.mostRecentChanges_capacitySize);

			var support7 = valuesChangesSupport[7] = valueChangeSupportArrayPool.Borrow();
            support7.baselineValue_current.UnityEngine_Vector3 = Transform.position; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support7.lastKnownValue.UnityEngine_Vector3 = Transform.position; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support7.lastKnownValue_previous.UnityEngine_Vector3 = Transform.position; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support7.valueLimitEncountered_min.UnityEngine_Vector3 = Transform.position; 
			support7.valueLimitEncountered_max.UnityEngine_Vector3 = Transform.position; 
			support7.lastKnownValue_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support7.lastKnownValue_previous_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support7.syncCompanion = this;
			support7.memberName = "position";
			support7.index = 7;
			support7.syncAttribute_MustRunOnUnityMainThread = true;
			support7.syncAttribute_ProcessingPriority = 0;
			support7.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support7.syncAttribute_SyncChangesEverySeconds = 0.04166667f;
			support7.syncAttribute_Reliability = AutoMagicalSyncReliability.Unreliable;
			support7.syncAttribute_ShouldBlendBetweenValuesReceived = true;
			support7.syncAttribute_PhysicsUpdateInterval = 1;
			support7.syncAttribute_EnablePhysicsSnapping = false;
			support7.isVelocityEligible = true;
			support7.syncAttribute_VelocityQuantizeLowerBound = -0.366233f;
			support7.syncAttribute_VelocityQuantizeUpperBound = 0.366233f;
			support7.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support7_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support7_effectiveSyncInterval = support7_isPhysicsObject && support7.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support7.syncAttribute_PhysicsUpdateInterval
				: support7.syncAttribute_SyncChangesEverySeconds;
			support7.velocityQuantizeLowerBound_PerSyncInterval = support7.syncAttribute_VelocityQuantizeLowerBound * support7_effectiveSyncInterval;
			support7.velocityQuantizeUpperBound_PerSyncInterval = support7.syncAttribute_VelocityQuantizeUpperBound * support7_effectiveSyncInterval;
			support7.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support7.codeGenerationMemberType = GONetSyncableValueTypes.UnityEngine_Vector3;
			support7.syncAttribute_ShouldSkipSync = GONetMain.IsPositionNotSyncd;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((2, 7), out support7.syncAttribute_ShouldSkipSync);
			support7.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-125f, 125f, 14, true);

			cachedCustomSerializers[7] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(14, -125f, 125f);
			cachedValueSerializers[7] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(14, -125f, 125f);
			cachedVelocitySerializers[7] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(14, -0.366233f, 0.366233f);

			cachedCustomValueBlendings[7] = GONetAutoMagicalSyncAttribute.GetCustomValueBlending<GONet.PluginAPI.GONetValueBlending_Vector3_HermiteSpline>();
			cachedCustomVelocityBlendings[7] = GONet.Utils.ValueBlendUtils.GetDefaultVelocityBlending(support7.lastKnownValue.GONetSyncType);
            int support7_mostRecentChanges_calcdSize = support7.syncAttribute_SyncChangesEverySeconds != 0 ? (int)((GONetMain.valueBlendingBufferLeadSeconds / support7.syncAttribute_SyncChangesEverySeconds) * 2.5f) : 0;
            support7.mostRecentChanges_capacitySize = Math.Max(support7_mostRecentChanges_calcdSize, GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.MOST_RECENT_CHANGEs_SIZE_MINIMUM);
			support7.mostRecentChanges = GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.mostRecentChangesPool.Borrow(support7.mostRecentChanges_capacitySize);

		}

        internal override void SetAutoMagicalSyncValue(byte index, GONetSyncableValue value)
		{
			switch (index)
			{
				case 0:
					GONetParticipant.GONetId = value.System_UInt32;
					return;
				case 1:
					GONetParticipant.IsPositionSyncd = value.System_Boolean;
					return;
				case 2:
					GONetParticipant.IsRotationSyncd = value.System_Boolean;
					return;
				case 3:
					GONetParticipant.OwnerAuthorityId = value.System_UInt16;
					return;
				case 4:
					GONetParticipant.RemotelyControlledByAuthorityId = value.System_UInt16;
					return;
				case 5:
					PlayerController.splitRotation = value.UnityEngine_Quaternion;
					return;
				case 6:
					Transform.rotation = value.UnityEngine_Quaternion;
					return;
				case 7:
					Transform.position = value.UnityEngine_Vector3;
					return;
			}
		}

        internal override GONetSyncableValue GetAutoMagicalSyncValue(byte index)
		{
			switch (index)
			{
				case 0:
					return GONetParticipant.GONetId;
				case 1:
					return GONetParticipant.IsPositionSyncd;
				case 2:
					return GONetParticipant.IsRotationSyncd;
				case 3:
					return GONetParticipant.OwnerAuthorityId;
				case 4:
					return GONetParticipant.RemotelyControlledByAuthorityId;
				case 5:
					return PlayerController.splitRotation;
				case 6:
					return Transform.rotation;
				case 7:
					return Transform.position;
			}

			return default;
		}

        /// <summary>
        /// Serializes all values of appropriaate member variables internally to <paramref name="bitStream_appendTo"/>.
        /// Oops.  Just kidding....it's ALMOST all values.  The exception being <see cref="GONetParticipant.GONetId"/> because that has to be processed first separately in order
        /// to know which <see cref="GONetParticipant"/> we are working with in order to call this method.
        /// </summary>
        internal override void SerializeAll(Utils.BitByBitByteArrayBuilder bitStream_appendTo)
        {
            // SerializeAll is INIT ONLY - always send VALUES (no velocity data during initial sync)

			{ // GONetParticipant.IsPositionSyncd
				bitStream_appendTo.WriteBit(GONetParticipant.IsPositionSyncd);
			}
			{ // GONetParticipant.IsRotationSyncd
				bitStream_appendTo.WriteBit(GONetParticipant.IsRotationSyncd);
			}
			{ // GONetParticipant.OwnerAuthorityId
				bitStream_appendTo.WriteUShort(GONetParticipant.OwnerAuthorityId);
			}
			{ // GONetParticipant.RemotelyControlledByAuthorityId
				bitStream_appendTo.WriteUShort(GONetParticipant.RemotelyControlledByAuthorityId);
			}
			{ // PlayerController.splitRotation
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[5];
				customSerializer.Serialize(bitStream_appendTo, gonetParticipant, PlayerController.splitRotation);
			}
			{ // Transform.rotation
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[6];
				customSerializer.Serialize(bitStream_appendTo, gonetParticipant, Transform.rotation);
			}
			{ // Transform.position
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[7];
				{ // SUB-QUANTIZATION DIAGNOSTIC for position
					var currentValue = Transform.position;
					var baselineValue = valuesChangesSupport[7].baselineValue_current.UnityEngine_Vector3;
					var deltaFromBaseline = currentValue - baselineValue;
					// GONet.Utils.SubQuantizationDiagnostics.CheckAndLogIfSubQuantization(gonetParticipant.GONetId, "position", deltaFromBaseline, valuesChangesSupport[7].syncAttribute_QuantizerSettingsGroup, customSerializer);
					customSerializer.Serialize(bitStream_appendTo, gonetParticipant, deltaFromBaseline);
				}
			}
        }

        internal override void SerializeSingle(Utils.BitByBitByteArrayBuilder bitStream_appendTo, byte singleIndex, bool isVelocityBundle = false)
        {
			switch (singleIndex)
			{
				case 0:
				{ // GONetParticipant.GONetId
					    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[0];
					customSerializer.Serialize(bitStream_appendTo, gonetParticipant, GONetParticipant.GONetId);
				}
				break;

				case 1:
				{ // GONetParticipant.IsPositionSyncd
					bitStream_appendTo.WriteBit(GONetParticipant.IsPositionSyncd);
				}
				break;

				case 2:
				{ // GONetParticipant.IsRotationSyncd
					bitStream_appendTo.WriteBit(GONetParticipant.IsRotationSyncd);
				}
				break;

				case 3:
				{ // GONetParticipant.OwnerAuthorityId
					bitStream_appendTo.WriteUShort(GONetParticipant.OwnerAuthorityId);
				}
				break;

				case 4:
				{ // GONetParticipant.RemotelyControlledByAuthorityId
					bitStream_appendTo.WriteUShort(GONetParticipant.RemotelyControlledByAuthorityId);
				}
				break;

				case 5:
				{ // PlayerController.splitRotation
					    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[5];
					// VelocitySyncTelemetry.TraceServerAuthority(gonetParticipant.GONetId, 5, "splitRotation", PlayerController.splitRotation);
					if (isVelocityBundle)
					{
						// VELOCITY BUNDLE: Send raw delta (no division - optimization!)
						var changesSupport = valuesChangesSupport[5];
						GONetSyncableValue velocityValue;
						
						// CRITICAL: Use lastKnownValue (authority's transform) NOT mostRecentChanges (client blending queue)
						var current = changesSupport.lastKnownValue;
						var previous = changesSupport.lastKnownValue_previous;
						
						// Check if we have valid previous value
						if (current.GONetSyncType == previous.GONetSyncType && current.GONetSyncType != GONet.GONetSyncableValueTypes.System_Boolean)
						{
							// Angular velocity for Quaternion (stored as Vector3)
							UnityEngine.Quaternion currentValue = current.UnityEngine_Quaternion;
							UnityEngine.Quaternion previousValue = previous.UnityEngine_Quaternion;
						
							// Calculate deltaTime from timestamps
							long deltaTimeTicks = changesSupport.lastKnownValue_elapsedTicks - changesSupport.lastKnownValue_previous_elapsedTicks;
							float deltaTime = (float)(deltaTimeTicks * GONet.Utils.HighResolutionTimeUtils.TICKS_TO_SECONDS);
						
							// Protect against zero deltaTime
							if (deltaTime <= 0f)
							{
								deltaTime = UnityEngine.Time.deltaTime > 0f ? UnityEngine.Time.deltaTime : 0.0167f; // 60 FPS fallback
							}
						
							// Use quaternion exponential map for angular velocity calculation
							UnityEngine.Quaternion deltaRotation = currentValue * UnityEngine.Quaternion.Inverse(previousValue);
							// Use Log_Exact() for precise angular velocity (eliminates polynomial approximation errors)
							UnityEngine.Quaternion logDelta = GONet.Utils.QuaternionUtilsOptimized.Log_Exact(deltaRotation);
							// logDelta contains half-angle rotation vector, multiply by 2 for full angular velocity
							UnityEngine.Vector3 angularVelocity = new UnityEngine.Vector3(logDelta.x * 2f, logDelta.y * 2f, logDelta.z * 2f) / deltaTime;
							velocityValue = new GONetSyncableValue();
							velocityValue.UnityEngine_Vector3 = angularVelocity;
							// VelocitySyncTelemetry.TrackVelocityCalculation(true, gonetParticipant.GONetId);
						}
						else
						{
							// Type mismatch, use zero delta
							GONet.GONetLog.Warning($"[VELOCITY-CALC-FALLBACK] GONetId={gonetParticipant.GONetId} currentType={current.GONetSyncType} previousType={previous.GONetSyncType} - Using zero velocity");
							velocityValue = new GONetSyncableValue();
							velocityValue.UnityEngine_Vector3 = UnityEngine.Vector3.zero;
							// VelocitySyncTelemetry.TrackVelocityCalculation(false, gonetParticipant.GONetId);
						}
						
						// Serialize velocity using velocity serializer
						cachedVelocitySerializers[5].Serialize(bitStream_appendTo, gonetParticipant, velocityValue);
						// VelocitySyncTelemetry.TrackVelocityBundleSent(gonetParticipant.GONetId);
						// VelocitySyncTelemetry.TraceServerSend(gonetParticipant.GONetId, 5, "VELOCITY", PlayerController.splitRotation, velocityValue.UnityEngine_Vector3, "partition_decision", valuesChangesSupport[5].mostRecentChanges_usedSize, true);
					}
					else
					{
						// VALUE BUNDLE: Serialize position normally
						customSerializer.Serialize(bitStream_appendTo, gonetParticipant, PlayerController.splitRotation);
				
						// Store snapshot for velocity calculation on next VELOCITY bundle
						var snapshotValue = new GONetSyncableValue();
						snapshotValue.UnityEngine_Quaternion = PlayerController.splitRotation;
						valuesChangesSupport[5].AddToMostRecentChangeQueue_IfAppropriate(GONet.GONetMain.Time.ElapsedTicks, snapshotValue);
						// VelocitySyncTelemetry.TrackValueBundleSent(gonetParticipant.GONetId);
						// VelocitySyncTelemetry.TraceServerSend(gonetParticipant.GONetId, 5, "VALUE", PlayerController.splitRotation, UnityEngine.Vector3.zero, "partition_decision", valuesChangesSupport[5].mostRecentChanges_usedSize, false);
					}
				}
				break;

				case 6:
				{ // Transform.rotation
					    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[6];
					// VelocitySyncTelemetry.TraceServerAuthority(gonetParticipant.GONetId, 6, "rotation", Transform.rotation);
					if (isVelocityBundle)
					{
						// VELOCITY BUNDLE: Send raw delta (no division - optimization!)
						var changesSupport = valuesChangesSupport[6];
						GONetSyncableValue velocityValue;
						
						// CRITICAL: Use lastKnownValue (authority's transform) NOT mostRecentChanges (client blending queue)
						var current = changesSupport.lastKnownValue;
						var previous = changesSupport.lastKnownValue_previous;
						
						// Check if we have valid previous value
						if (current.GONetSyncType == previous.GONetSyncType && current.GONetSyncType != GONet.GONetSyncableValueTypes.System_Boolean)
						{
							// Angular velocity for Quaternion (stored as Vector3)
							UnityEngine.Quaternion currentValue = current.UnityEngine_Quaternion;
							UnityEngine.Quaternion previousValue = previous.UnityEngine_Quaternion;
						
							// Calculate deltaTime from timestamps
							long deltaTimeTicks = changesSupport.lastKnownValue_elapsedTicks - changesSupport.lastKnownValue_previous_elapsedTicks;
							float deltaTime = (float)(deltaTimeTicks * GONet.Utils.HighResolutionTimeUtils.TICKS_TO_SECONDS);
						
							// Protect against zero deltaTime
							if (deltaTime <= 0f)
							{
								deltaTime = UnityEngine.Time.deltaTime > 0f ? UnityEngine.Time.deltaTime : 0.0167f; // 60 FPS fallback
							}
						
							// Use quaternion exponential map for angular velocity calculation
							UnityEngine.Quaternion deltaRotation = currentValue * UnityEngine.Quaternion.Inverse(previousValue);
							// Use Log_Exact() for precise angular velocity (eliminates polynomial approximation errors)
							UnityEngine.Quaternion logDelta = GONet.Utils.QuaternionUtilsOptimized.Log_Exact(deltaRotation);
							// logDelta contains half-angle rotation vector, multiply by 2 for full angular velocity
							UnityEngine.Vector3 angularVelocity = new UnityEngine.Vector3(logDelta.x * 2f, logDelta.y * 2f, logDelta.z * 2f) / deltaTime;
							velocityValue = new GONetSyncableValue();
							velocityValue.UnityEngine_Vector3 = angularVelocity;
							// VelocitySyncTelemetry.TrackVelocityCalculation(true, gonetParticipant.GONetId);
						}
						else
						{
							// Type mismatch, use zero delta
							GONet.GONetLog.Warning($"[VELOCITY-CALC-FALLBACK] GONetId={gonetParticipant.GONetId} currentType={current.GONetSyncType} previousType={previous.GONetSyncType} - Using zero velocity");
							velocityValue = new GONetSyncableValue();
							velocityValue.UnityEngine_Vector3 = UnityEngine.Vector3.zero;
							// VelocitySyncTelemetry.TrackVelocityCalculation(false, gonetParticipant.GONetId);
						}
						
						// Serialize velocity using velocity serializer
						cachedVelocitySerializers[6].Serialize(bitStream_appendTo, gonetParticipant, velocityValue);
						// VelocitySyncTelemetry.TrackVelocityBundleSent(gonetParticipant.GONetId);
						// VelocitySyncTelemetry.TraceServerSend(gonetParticipant.GONetId, 6, "VELOCITY", Transform.rotation, velocityValue.UnityEngine_Vector3, "partition_decision", valuesChangesSupport[6].mostRecentChanges_usedSize, true);
					}
					else
					{
						// VALUE BUNDLE: Serialize position normally
						customSerializer.Serialize(bitStream_appendTo, gonetParticipant, Transform.rotation);
				
						// Store snapshot for velocity calculation on next VELOCITY bundle
						var snapshotValue = new GONetSyncableValue();
						snapshotValue.UnityEngine_Quaternion = Transform.rotation;
						valuesChangesSupport[6].AddToMostRecentChangeQueue_IfAppropriate(GONet.GONetMain.Time.ElapsedTicks, snapshotValue);
						// VelocitySyncTelemetry.TrackValueBundleSent(gonetParticipant.GONetId);
						// VelocitySyncTelemetry.TraceServerSend(gonetParticipant.GONetId, 6, "VALUE", Transform.rotation, UnityEngine.Vector3.zero, "partition_decision", valuesChangesSupport[6].mostRecentChanges_usedSize, false);
					}
				}
				break;

				case 7:
				{ // Transform.position
					    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[7];
					if (isVelocityBundle)
					{
						// VELOCITY BUNDLE: Send raw delta (no division - optimization!)
						var changesSupport = valuesChangesSupport[7];
						GONetSyncableValue velocityValue;
						
						// CRITICAL: Use lastKnownValue (authority's transform) NOT mostRecentChanges (client blending queue)
						var current = changesSupport.lastKnownValue;
						var previous = changesSupport.lastKnownValue_previous;
						
						// Check if we have valid previous value
						if (current.GONetSyncType == previous.GONetSyncType && current.GONetSyncType != GONet.GONetSyncableValueTypes.System_Boolean)
						{
							// Calculate velocity from position change (units/sec)
							UnityEngine.Vector3 currentValue = current.UnityEngine_Vector3;
							UnityEngine.Vector3 previousValue = previous.UnityEngine_Vector3;
						
							// Calculate deltaTime from timestamps
							long deltaTimeTicks = changesSupport.lastKnownValue_elapsedTicks - changesSupport.lastKnownValue_previous_elapsedTicks;
							float deltaTime = (float)(deltaTimeTicks * GONet.Utils.HighResolutionTimeUtils.TICKS_TO_SECONDS);
						
							// Protect against zero deltaTime
							if (deltaTime <= 0f)
							{
								deltaTime = UnityEngine.Time.deltaTime > 0f ? UnityEngine.Time.deltaTime : 0.0167f; // 60 FPS fallback
							}
						
							velocityValue = new GONetSyncableValue();
							UnityEngine.Vector3 deltaValue = currentValue - previousValue;
							UnityEngine.Vector3 velocity = deltaValue / deltaTime;
							velocityValue.UnityEngine_Vector3 = velocity;
						
							// VelocitySyncTelemetry.TrackVelocityCalculation(true, gonetParticipant.GONetId);
							// VelocitySyncTelemetry.TrackVelocityCalculation(true, gonetParticipant.GONetId);
						}
						else
						{
							// Type mismatch, use zero delta
							GONet.GONetLog.Warning($"[VELOCITY-CALC-FALLBACK] GONetId={gonetParticipant.GONetId} currentType={current.GONetSyncType} previousType={previous.GONetSyncType} - Using zero velocity");
							velocityValue = new GONetSyncableValue();
							velocityValue.UnityEngine_Vector3 = UnityEngine.Vector3.zero;
							// VelocitySyncTelemetry.TrackVelocityCalculation(false, gonetParticipant.GONetId);
						}
						
						// Serialize velocity using velocity serializer
						cachedVelocitySerializers[7].Serialize(bitStream_appendTo, gonetParticipant, velocityValue);
						// VelocitySyncTelemetry.TrackVelocityBundleSent(gonetParticipant.GONetId);
					}
					else
					{
						// VALUE BUNDLE: Serialize position normally
						{ // SUB-QUANTIZATION DIAGNOSTIC for position
							var currentValue = Transform.position;
							var baselineValue = valuesChangesSupport[7].baselineValue_current.UnityEngine_Vector3;
							var deltaFromBaseline = currentValue - baselineValue;
							// GONet.Utils.SubQuantizationDiagnostics.CheckAndLogIfSubQuantization(gonetParticipant.GONetId, "position", deltaFromBaseline, valuesChangesSupport[7].syncAttribute_QuantizerSettingsGroup, customSerializer);
							customSerializer.Serialize(bitStream_appendTo, gonetParticipant, deltaFromBaseline);
						}
				
						// Store snapshot for velocity calculation on next VELOCITY bundle
						var snapshotValue = new GONetSyncableValue();
						snapshotValue.UnityEngine_Vector3 = Transform.position;
						valuesChangesSupport[7].AddToMostRecentChangeQueue_IfAppropriate(GONet.GONetMain.Time.ElapsedTicks, snapshotValue);
						// VelocitySyncTelemetry.TrackValueBundleSent(gonetParticipant.GONetId);
					}
				}
				break;

			}
        }

        /// <summary>
        /// PRE: value at <paramref name="singleIndex"/> is known to be configured to be quantized
        /// NOTE: This is only virtual to avoid upgrading customers prior to this being added having compilation issues when upgrading from a previous version of GONet
        /// </summary>
        protected override bool AreEqualQuantized(byte singleIndex, GONetSyncableValue valueA, GONetSyncableValue valueB)
		{
			switch (singleIndex)
			{
				case 0:
				{ // GONetParticipant.GONetId
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[0];
					return customSerializer.AreEqualConsideringQuantization(valueA, valueB);
				}
				break;

				case 1:
				{ // GONetParticipant.IsPositionSyncd
					return valueA.System_Boolean == valueB.System_Boolean;
				}
				break;

				case 2:
				{ // GONetParticipant.IsRotationSyncd
					return valueA.System_Boolean == valueB.System_Boolean;
				}
				break;

				case 3:
				{ // GONetParticipant.OwnerAuthorityId
					// handle quantization of this type eventually?
				}
				break;

				case 4:
				{ // GONetParticipant.RemotelyControlledByAuthorityId
					// handle quantization of this type eventually?
				}
				break;

				case 5:
				{ // PlayerController.splitRotation
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[5];
					return customSerializer.AreEqualConsideringQuantization(valueA, valueB);
				}
				break;

				case 6:
				{ // Transform.rotation
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[6];
					return customSerializer.AreEqualConsideringQuantization(valueA, valueB);
				}
				break;

				case 7:
				{ // Transform.position
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[7];
					return customSerializer.AreEqualConsideringQuantization(valueA, valueB);
				}
				break;

			}

			return base.AreEqualQuantized(singleIndex, valueA, valueB);
		}

        /// <summary>
        /// Deserializes all values from <paramref name="bitStream_readFrom"/> and uses them to modify appropriate member variables internally.
        /// Oops.  Just kidding....it's ALMOST all values.  The exception being <see cref="GONetParticipant.GONetId"/> because that has to be processed first separately in order
        /// to know which <see cref="GONetParticipant"/> we are working with in order to call this method.
        /// </summary>
        internal override void DeserializeInitAll(Utils.BitByBitByteArrayBuilder bitStream_readFrom, long assumedElapsedTicksAtChange)
        {
			{ // GONetParticipant.IsPositionSyncd
				bool value;
			                bitStream_readFrom.ReadBit(out value);
				GONetParticipant.IsPositionSyncd = value;
			}
			{ // GONetParticipant.IsRotationSyncd
				bool value;
			                bitStream_readFrom.ReadBit(out value);
				GONetParticipant.IsRotationSyncd = value;
			}
			{ // GONetParticipant.OwnerAuthorityId
				ushort value;
			                bitStream_readFrom.ReadUShort(out value);
				GONetParticipant.OwnerAuthorityId = value;
			}
			{ // GONetParticipant.RemotelyControlledByAuthorityId
				ushort value;
			                bitStream_readFrom.ReadUShort(out value);
				GONetParticipant.RemotelyControlledByAuthorityId = value;
			}
			{ // PlayerController.splitRotation
				IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[5];
				var value = customSerializer.Deserialize(bitStream_readFrom).UnityEngine_Quaternion;
				PlayerController.splitRotation = value;
			}
			{ // Transform.rotation
				IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[6];
				var value = customSerializer.Deserialize(bitStream_readFrom).UnityEngine_Quaternion;
				Transform.rotation = value;
			}
			{ // Transform.position
				IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[7];
				var value = customSerializer.Deserialize(bitStream_readFrom).UnityEngine_Vector3;
				value += valuesChangesSupport[7].baselineValue_current.UnityEngine_Vector3;
				Transform.position = value;
			}
        }


        /// <summary>
        /// Simply deserializes in order to move along the bit stream counter, but does NOT apply the values (i.e, does NOT init).
        /// Velocity-augmented sync: Supports deserializing either VALUE or VELOCITY data based on useVelocitySerializer parameter.
        /// </summary>
        /// <param name="bitStream_readFrom">The bit stream to deserialize from</param>
        /// <param name="singleIndex">The index of the value to deserialize</param>
        /// <param name="useVelocitySerializer">If true, uses cachedVelocitySerializers; if false, uses cachedValueSerializers. Default false.</param>
        /// <returns>The deserialized value (either VALUE or VELOCITY depending on useVelocitySerializer)</returns>
        internal override GONet.GONetSyncableValue DeserializeInitSingle_ReadOnlyNotApply(Utils.BitByBitByteArrayBuilder bitStream_readFrom, byte singleIndex, bool useVelocitySerializer = false)
        {
			switch (singleIndex)
			{
				case 0:
				{ // GONetParticipant.GONetId
					// Velocity-augmented sync: Choose serializer based on packet type (VALUE vs VELOCITY)
					IGONetAutoMagicalSync_CustomSerializer customSerializer = useVelocitySerializer ? cachedVelocitySerializers[0] : cachedValueSerializers[0];
					GONetSyncableValue value = customSerializer.Deserialize(bitStream_readFrom);
					var typedValue = value.System_UInt32;
					value.System_UInt32 = typedValue;
					return value;
				}
				case 1:
				{ // GONetParticipant.IsPositionSyncd
					bool value;
				                bitStream_readFrom.ReadBit(out value);
					return value;
				}
				case 2:
				{ // GONetParticipant.IsRotationSyncd
					bool value;
				                bitStream_readFrom.ReadBit(out value);
					return value;
				}
				case 3:
				{ // GONetParticipant.OwnerAuthorityId
					ushort value;
				                bitStream_readFrom.ReadUShort(out value);
					return value;
				}
				case 4:
				{ // GONetParticipant.RemotelyControlledByAuthorityId
					ushort value;
				                bitStream_readFrom.ReadUShort(out value);
					return value;
				}
				case 5:
				{ // PlayerController.splitRotation
					// Velocity-augmented sync: Choose serializer based on packet type (VALUE vs VELOCITY)
					IGONetAutoMagicalSync_CustomSerializer customSerializer = useVelocitySerializer ? cachedVelocitySerializers[5] : cachedValueSerializers[5];
					GONetSyncableValue value = customSerializer.Deserialize(bitStream_readFrom);
					if (useVelocitySerializer)
					{
						// VELOCITY bundle: Contains Vector3 angular velocity (rad/s)
						var typedValue = value.UnityEngine_Vector3;
						value.UnityEngine_Vector3 = typedValue;
					}
					else
					{
						// VALUE bundle: Contains Quaternion rotation
						var typedValue = value.UnityEngine_Quaternion;
						value.UnityEngine_Quaternion = typedValue;
					}
					return value;
				}
				case 6:
				{ // Transform.rotation
					// Velocity-augmented sync: Choose serializer based on packet type (VALUE vs VELOCITY)
					IGONetAutoMagicalSync_CustomSerializer customSerializer = useVelocitySerializer ? cachedVelocitySerializers[6] : cachedValueSerializers[6];
					GONetSyncableValue value = customSerializer.Deserialize(bitStream_readFrom);
					if (useVelocitySerializer)
					{
						// VELOCITY bundle: Contains Vector3 angular velocity (rad/s)
						var typedValue = value.UnityEngine_Vector3;
						value.UnityEngine_Vector3 = typedValue;
					}
					else
					{
						// VALUE bundle: Contains Quaternion rotation
						var typedValue = value.UnityEngine_Quaternion;
						value.UnityEngine_Quaternion = typedValue;
					}
					return value;
				}
				case 7:
				{ // Transform.position
					// Velocity-augmented sync: Choose serializer based on packet type (VALUE vs VELOCITY)
					IGONetAutoMagicalSync_CustomSerializer customSerializer = useVelocitySerializer ? cachedVelocitySerializers[7] : cachedValueSerializers[7];
					GONetSyncableValue value = customSerializer.Deserialize(bitStream_readFrom);
					var typedValue = value.UnityEngine_Vector3;
					if (!useVelocitySerializer)
					{
						typedValue += valuesChangesSupport[7].baselineValue_current.UnityEngine_Vector3;
					}
					value.UnityEngine_Vector3 = typedValue;
					return value;
				}
			}

			return default;
        }


        /// <summary>
        /// Deserializes and initializes a single value.
        /// Velocity-augmented sync: For VELOCITY bundles, synthesizes position from velocity before applying.
        /// </summary>
        internal override void DeserializeInitSingle(Utils.BitByBitByteArrayBuilder bitStream_readFrom, byte singleIndex, long assumedElapsedTicksAtChange, bool useVelocitySerializer = false)
        {
            GONetSyncableValue value = DeserializeInitSingle_ReadOnlyNotApply(bitStream_readFrom, singleIndex, useVelocitySerializer);

            // NOTE: Velocity synthesis removed - GONet.cs handles synthesis manually via static method
            // This code path (DeserializeInitSingle with synthesis) is never actually reached in practice.
            // GONet.cs calls DeserializeInitSingle_ReadOnlyNotApply and does synthesis itself.

            InitSingle(value, singleIndex, assumedElapsedTicksAtChange);
        }

        internal override void InitSingle(GONetSyncableValue value, byte singleIndex, long assumedElapsedTicksAtChange)
        {
			switch (singleIndex)
			{
				case 0:					GONetParticipant.GONetId = value.System_UInt32; break;
				case 1:					valuesChangesSupport[1].AddToMostRecentChangeQueue_IfAppropriate(assumedElapsedTicksAtChange, value); break; // NOTE: this queue will be used each frame to blend between this value and others added there
				case 2:					valuesChangesSupport[2].AddToMostRecentChangeQueue_IfAppropriate(assumedElapsedTicksAtChange, value); break; // NOTE: this queue will be used each frame to blend between this value and others added there
				case 3:					GONetParticipant.OwnerAuthorityId = value.System_UInt16; break;
				case 4:					GONetParticipant.RemotelyControlledByAuthorityId = value.System_UInt16; break;
				case 5:					valuesChangesSupport[5].AddToMostRecentChangeQueue_IfAppropriate(assumedElapsedTicksAtChange, value); break; // NOTE: this queue will be used each frame to blend between this value and others added there
				case 6:					valuesChangesSupport[6].AddToMostRecentChangeQueue_IfAppropriate(assumedElapsedTicksAtChange, value); break; // NOTE: this queue will be used each frame to blend between this value and others added there
				case 7:					valuesChangesSupport[7].AddToMostRecentChangeQueue_IfAppropriate(assumedElapsedTicksAtChange, value); break; // NOTE: this queue will be used each frame to blend between this value and others added there
			}
        }

		internal override void UpdateLastKnownValues(GONetMain.SyncBundleUniqueGrouping onlyMatchIfUniqueGroupingMatches)
		{
				var valuesChangesSupport0 = valuesChangesSupport[0];
				if (DoesMatchUniqueGrouping(valuesChangesSupport0, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport0, 0)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					valuesChangesSupport0.lastKnownValue.System_UInt32 = GONetParticipant.GONetId;
					valuesChangesSupport0.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport1 = valuesChangesSupport[1];
				if (DoesMatchUniqueGrouping(valuesChangesSupport1, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport1, 1)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					valuesChangesSupport1.lastKnownValue.System_Boolean = GONetParticipant.IsPositionSyncd;
					valuesChangesSupport1.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport2 = valuesChangesSupport[2];
				if (DoesMatchUniqueGrouping(valuesChangesSupport2, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport2, 2)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					valuesChangesSupport2.lastKnownValue.System_Boolean = GONetParticipant.IsRotationSyncd;
					valuesChangesSupport2.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport3 = valuesChangesSupport[3];
				if (DoesMatchUniqueGrouping(valuesChangesSupport3, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport3, 3)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					valuesChangesSupport3.lastKnownValue.System_UInt16 = GONetParticipant.OwnerAuthorityId;
					valuesChangesSupport3.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport4 = valuesChangesSupport[4];
				if (DoesMatchUniqueGrouping(valuesChangesSupport4, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport4, 4)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					valuesChangesSupport4.lastKnownValue.System_UInt16 = GONetParticipant.RemotelyControlledByAuthorityId;
					valuesChangesSupport4.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport5 = valuesChangesSupport[5];
				if (DoesMatchUniqueGrouping(valuesChangesSupport5, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport5, 5)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					// Velocity-eligible: Save OLD current to previous BEFORE updating current
					valuesChangesSupport5.lastKnownValue_previous_elapsedTicks = valuesChangesSupport5.lastKnownValue_elapsedTicks;
					valuesChangesSupport5.lastKnownValue_previous = valuesChangesSupport5.lastKnownValue;

					valuesChangesSupport5.lastKnownValue.UnityEngine_Quaternion = PlayerController.splitRotation;
					valuesChangesSupport5.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport6 = valuesChangesSupport[6];
				if (DoesMatchUniqueGrouping(valuesChangesSupport6, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport6, 6)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					// Velocity-eligible: Save OLD current to previous BEFORE updating current
					valuesChangesSupport6.lastKnownValue_previous_elapsedTicks = valuesChangesSupport6.lastKnownValue_elapsedTicks;
					valuesChangesSupport6.lastKnownValue_previous = valuesChangesSupport6.lastKnownValue;

					if (shouldSourceFromRigidbody)
					{
						// Source from Rigidbody (physics simulation)
						valuesChangesSupport6.lastKnownValue.UnityEngine_Quaternion = gonetParticipant.myRigidBody.rotation;
					}
					else
					{
						// Source from Transform (regular sync)
						valuesChangesSupport6.lastKnownValue.UnityEngine_Quaternion = Transform.rotation;
					}
					valuesChangesSupport6.lastKnownValue_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport7 = valuesChangesSupport[7];
				if (DoesMatchUniqueGrouping(valuesChangesSupport7, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport7, 7)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					// Velocity-eligible: Save OLD current to previous BEFORE updating current
					valuesChangesSupport7.lastKnownValue_previous_elapsedTicks = valuesChangesSupport7.lastKnownValue_elapsedTicks;
					valuesChangesSupport7.lastKnownValue_previous = valuesChangesSupport7.lastKnownValue;

					if (shouldSourceFromRigidbody)
					{
						// Source from Rigidbody (physics simulation)
						valuesChangesSupport7.lastKnownValue.UnityEngine_Vector3 = gonetParticipant.myRigidBody.position;
					}
					else
					{
						// Source from Transform (regular sync)
						valuesChangesSupport7.lastKnownValue.UnityEngine_Vector3 = Transform.position;
					}
					valuesChangesSupport7.lastKnownValue_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks;
				}

		}

		internal override bool IsLastKnownValue_VeryCloseTo_Or_AlreadyOutsideOf_QuantizationRange(byte singleIndex, GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue valueChangeSupport)
		{
			switch (singleIndex)
			{
				case 0:
				{ // GONetParticipant.GONetId
					// this type not supported for this functionality
				}
				break;

				case 1:
				{ // GONetParticipant.IsPositionSyncd
					// this type not supported for this functionality
				}
				break;

				case 2:
				{ // GONetParticipant.IsRotationSyncd
					// this type not supported for this functionality
				}
				break;

				case 3:
				{ // GONetParticipant.OwnerAuthorityId
					// this type not supported for this functionality
				}
				break;

				case 4:
				{ // GONetParticipant.RemotelyControlledByAuthorityId
					// this type not supported for this functionality
				}
				break;

				case 5:
				{ // PlayerController.splitRotation
					// this type not supported for this functionality
				}
				break;

				case 6:
				{ // Transform.rotation
					// this type not supported for this functionality
				}
				break;

				case 7:
				{ // Transform.position
                    UnityEngine.Vector3 diff = valueChangeSupport.lastKnownValue.UnityEngine_Vector3 - valueChangeSupport.baselineValue_current.UnityEngine_Vector3;
					System.Single componentLimitLower = valueChangeSupport.syncAttribute_QuantizerSettingsGroup.lowerBound * 0.8f; // TODO cache this value
					System.Single componentLimitUpper = valueChangeSupport.syncAttribute_QuantizerSettingsGroup.upperBound * 0.8f; // TODO cache this value
                    bool isVeryCloseTo_Or_AlreadyOutsideOf_QuantizationRange = 
						diff.x < componentLimitLower || diff.x > componentLimitUpper ||
						diff.y < componentLimitLower || diff.y > componentLimitUpper ||
						diff.z < componentLimitLower || diff.z > componentLimitUpper;
					return isVeryCloseTo_Or_AlreadyOutsideOf_QuantizationRange;
				}
				break;

			}

			return false;
		}

        internal override ValueMonitoringSupport_NewBaselineEvent CreateNewBaselineValueEvent(uint gonetId, byte singleIndex, GONetSyncableValue newBaselineValue)
		{
			switch (singleIndex)
			{
				case 0:
				{ // GONetParticipant.GONetId
					return new ValueMonitoringSupport_NewBaselineEvent_System_UInt32() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.System_UInt32
					};
				}

				case 1:
				{ // GONetParticipant.IsPositionSyncd
					return new ValueMonitoringSupport_NewBaselineEvent_System_Boolean() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.System_Boolean
					};
				}

				case 2:
				{ // GONetParticipant.IsRotationSyncd
					return new ValueMonitoringSupport_NewBaselineEvent_System_Boolean() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.System_Boolean
					};
				}

				case 3:
				{ // GONetParticipant.OwnerAuthorityId
					return new ValueMonitoringSupport_NewBaselineEvent_System_UInt16() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.System_UInt16
					};
				}

				case 4:
				{ // GONetParticipant.RemotelyControlledByAuthorityId
					return new ValueMonitoringSupport_NewBaselineEvent_System_UInt16() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.System_UInt16
					};
				}

				case 5:
				{ // PlayerController.splitRotation
					return new ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Quaternion() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.UnityEngine_Quaternion
					};
				}

				case 6:
				{ // Transform.rotation
					return new ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Quaternion() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.UnityEngine_Quaternion
					};
				}

				case 7:
				{ // Transform.position
					return new ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector3() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.UnityEngine_Vector3
					};
				}

			}

			return null;
		}
    }
}
