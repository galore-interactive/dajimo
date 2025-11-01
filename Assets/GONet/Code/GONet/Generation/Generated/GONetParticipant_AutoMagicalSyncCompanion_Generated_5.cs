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
	internal sealed class GONetParticipant_AutoMagicalSyncCompanion_Generated_5 : GONetParticipant_AutoMagicalSyncCompanion_Generated
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

		private EntityStateSynchronizer _EntityStateSynchronizer;
		internal EntityStateSynchronizer EntityStateSynchronizer
		{
			get
			{
				if ((object)_EntityStateSynchronizer == null)
				{
					_EntityStateSynchronizer = gonetParticipant.GetComponent<EntityStateSynchronizer>();
				}
				return _EntityStateSynchronizer;
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

        internal override byte CodeGenerationId => 5;

        internal GONetParticipant_AutoMagicalSyncCompanion_Generated_5(GONetParticipant gonetParticipant) : base(gonetParticipant)
		{
			valuesCount = 13;
			
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
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 0), out support0.syncAttribute_ShouldSkipSync);
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
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 1), out support1.syncAttribute_ShouldSkipSync);
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
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 2), out support2.syncAttribute_ShouldSkipSync);
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
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 3), out support3.syncAttribute_ShouldSkipSync);
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
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 4), out support4.syncAttribute_ShouldSkipSync);
			support4.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);


			var support5 = valuesChangesSupport[5] = valueChangeSupportArrayPool.Borrow();
            support5.baselineValue_current.UnityEngine_Vector3 = EntityStateSynchronizer.cameraLookAtEulerAngles; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support5.lastKnownValue.UnityEngine_Vector3 = EntityStateSynchronizer.cameraLookAtEulerAngles; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support5.lastKnownValue_previous.UnityEngine_Vector3 = EntityStateSynchronizer.cameraLookAtEulerAngles; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support5.valueLimitEncountered_min.UnityEngine_Vector3 = EntityStateSynchronizer.cameraLookAtEulerAngles; 
			support5.valueLimitEncountered_max.UnityEngine_Vector3 = EntityStateSynchronizer.cameraLookAtEulerAngles; 
			support5.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support5.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support5.syncCompanion = this;
			support5.memberName = "cameraLookAtEulerAngles";
			support5.index = 5;
			support5.syncAttribute_MustRunOnUnityMainThread = true;
			support5.syncAttribute_ProcessingPriority = 0;
			support5.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support5.syncAttribute_SyncChangesEverySeconds = 0.03125f;
			support5.syncAttribute_Reliability = AutoMagicalSyncReliability.Unreliable;
			support5.syncAttribute_ShouldBlendBetweenValuesReceived = true;
			support5.syncAttribute_PhysicsUpdateInterval = 1;
			support5.syncAttribute_EnablePhysicsSnapping = false;
			support5.isVelocityEligible = false;
			support5.syncAttribute_VelocityQuantizeLowerBound = -20.000000f;
			support5.syncAttribute_VelocityQuantizeUpperBound = 20.000000f;
			support5.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support5_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support5_effectiveSyncInterval = support5_isPhysicsObject && support5.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support5.syncAttribute_PhysicsUpdateInterval
				: support5.syncAttribute_SyncChangesEverySeconds;
			support5.velocityQuantizeLowerBound_PerSyncInterval = support5.syncAttribute_VelocityQuantizeLowerBound * support5_effectiveSyncInterval;
			support5.velocityQuantizeUpperBound_PerSyncInterval = support5.syncAttribute_VelocityQuantizeUpperBound * support5_effectiveSyncInterval;
			support5.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support5.codeGenerationMemberType = GONetSyncableValueTypes.UnityEngine_Vector3;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 5), out support5.syncAttribute_ShouldSkipSync);
			support5.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-360f, 360f, 0, true);

			cachedCustomSerializers[5] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(0, -360f, 360f);
			cachedValueSerializers[5] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(0, -360f, 360f);
			cachedVelocitySerializers[5] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(18, -20.000000f, 20.000000f);

			cachedCustomValueBlendings[5] = GONetAutoMagicalSyncAttribute.GetCustomValueBlending<GONet.PluginAPI.GONetValueBlending_Vector3_ExtrapolateWithLowPassSmoothingFilter>();
			cachedCustomVelocityBlendings[5] = GONet.Utils.ValueBlendUtils.GetDefaultVelocityBlending(support5.lastKnownValue.GONetSyncType);
            int support5_mostRecentChanges_calcdSize = support5.syncAttribute_SyncChangesEverySeconds != 0 ? (int)((GONetMain.valueBlendingBufferLeadSeconds / support5.syncAttribute_SyncChangesEverySeconds) * 2.5f) : 0;
            support5.mostRecentChanges_capacitySize = Math.Max(support5_mostRecentChanges_calcdSize, GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.MOST_RECENT_CHANGEs_SIZE_MINIMUM);
			support5.mostRecentChanges = GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.mostRecentChangesPool.Borrow(support5.mostRecentChanges_capacitySize);

			var support6 = valuesChangesSupport[6] = valueChangeSupportArrayPool.Borrow();
            support6.baselineValue_current.System_Boolean = EntityStateSynchronizer.isCrouched; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support6.lastKnownValue.System_Boolean = EntityStateSynchronizer.isCrouched; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support6.lastKnownValue_previous.System_Boolean = EntityStateSynchronizer.isCrouched; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support6.valueLimitEncountered_min.System_Boolean = EntityStateSynchronizer.isCrouched; 
			support6.valueLimitEncountered_max.System_Boolean = EntityStateSynchronizer.isCrouched; 
			support6.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support6.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support6.syncCompanion = this;
			support6.memberName = "isCrouched";
			support6.index = 6;
			support6.syncAttribute_MustRunOnUnityMainThread = false;
			support6.syncAttribute_ProcessingPriority = 0;
			support6.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support6.syncAttribute_SyncChangesEverySeconds = 0f;
			support6.syncAttribute_Reliability = AutoMagicalSyncReliability.Reliable;
			support6.syncAttribute_ShouldBlendBetweenValuesReceived = false;
			support6.syncAttribute_PhysicsUpdateInterval = 1;
			support6.syncAttribute_EnablePhysicsSnapping = false;
			support6.isVelocityEligible = false;
			support6.syncAttribute_VelocityQuantizeLowerBound = -20.000000f;
			support6.syncAttribute_VelocityQuantizeUpperBound = 20.000000f;
			support6.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support6_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support6_effectiveSyncInterval = support6_isPhysicsObject && support6.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support6.syncAttribute_PhysicsUpdateInterval
				: support6.syncAttribute_SyncChangesEverySeconds;
			support6.velocityQuantizeLowerBound_PerSyncInterval = support6.syncAttribute_VelocityQuantizeLowerBound * support6_effectiveSyncInterval;
			support6.velocityQuantizeUpperBound_PerSyncInterval = support6.syncAttribute_VelocityQuantizeUpperBound * support6_effectiveSyncInterval;
			support6.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support6.codeGenerationMemberType = GONetSyncableValueTypes.System_Boolean;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 6), out support6.syncAttribute_ShouldSkipSync);
			support6.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);


			var support7 = valuesChangesSupport[7] = valueChangeSupportArrayPool.Borrow();
            support7.baselineValue_current.System_Boolean = EntityStateSynchronizer.isGrounded; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support7.lastKnownValue.System_Boolean = EntityStateSynchronizer.isGrounded; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support7.lastKnownValue_previous.System_Boolean = EntityStateSynchronizer.isGrounded; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support7.valueLimitEncountered_min.System_Boolean = EntityStateSynchronizer.isGrounded; 
			support7.valueLimitEncountered_max.System_Boolean = EntityStateSynchronizer.isGrounded; 
			support7.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support7.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support7.syncCompanion = this;
			support7.memberName = "isGrounded";
			support7.index = 7;
			support7.syncAttribute_MustRunOnUnityMainThread = false;
			support7.syncAttribute_ProcessingPriority = 0;
			support7.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support7.syncAttribute_SyncChangesEverySeconds = 0f;
			support7.syncAttribute_Reliability = AutoMagicalSyncReliability.Reliable;
			support7.syncAttribute_ShouldBlendBetweenValuesReceived = false;
			support7.syncAttribute_PhysicsUpdateInterval = 1;
			support7.syncAttribute_EnablePhysicsSnapping = false;
			support7.isVelocityEligible = false;
			support7.syncAttribute_VelocityQuantizeLowerBound = -20.000000f;
			support7.syncAttribute_VelocityQuantizeUpperBound = 20.000000f;
			support7.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support7_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support7_effectiveSyncInterval = support7_isPhysicsObject && support7.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support7.syncAttribute_PhysicsUpdateInterval
				: support7.syncAttribute_SyncChangesEverySeconds;
			support7.velocityQuantizeLowerBound_PerSyncInterval = support7.syncAttribute_VelocityQuantizeLowerBound * support7_effectiveSyncInterval;
			support7.velocityQuantizeUpperBound_PerSyncInterval = support7.syncAttribute_VelocityQuantizeUpperBound * support7_effectiveSyncInterval;
			support7.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support7.codeGenerationMemberType = GONetSyncableValueTypes.System_Boolean;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 7), out support7.syncAttribute_ShouldSkipSync);
			support7.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);


			var support8 = valuesChangesSupport[8] = valueChangeSupportArrayPool.Borrow();
            support8.baselineValue_current.UnityEngine_Vector2 = EntityStateSynchronizer.movementInput; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support8.lastKnownValue.UnityEngine_Vector2 = EntityStateSynchronizer.movementInput; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support8.lastKnownValue_previous.UnityEngine_Vector2 = EntityStateSynchronizer.movementInput; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support8.valueLimitEncountered_min.UnityEngine_Vector2 = EntityStateSynchronizer.movementInput; 
			support8.valueLimitEncountered_max.UnityEngine_Vector2 = EntityStateSynchronizer.movementInput; 
			support8.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support8.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support8.syncCompanion = this;
			support8.memberName = "movementInput";
			support8.index = 8;
			support8.syncAttribute_MustRunOnUnityMainThread = true;
			support8.syncAttribute_ProcessingPriority = 0;
			support8.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support8.syncAttribute_SyncChangesEverySeconds = 0.03125f;
			support8.syncAttribute_Reliability = AutoMagicalSyncReliability.Unreliable;
			support8.syncAttribute_ShouldBlendBetweenValuesReceived = true;
			support8.syncAttribute_PhysicsUpdateInterval = 1;
			support8.syncAttribute_EnablePhysicsSnapping = false;
			support8.isVelocityEligible = false;
			support8.syncAttribute_VelocityQuantizeLowerBound = -20.000000f;
			support8.syncAttribute_VelocityQuantizeUpperBound = 20.000000f;
			support8.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support8_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support8_effectiveSyncInterval = support8_isPhysicsObject && support8.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support8.syncAttribute_PhysicsUpdateInterval
				: support8.syncAttribute_SyncChangesEverySeconds;
			support8.velocityQuantizeLowerBound_PerSyncInterval = support8.syncAttribute_VelocityQuantizeLowerBound * support8_effectiveSyncInterval;
			support8.velocityQuantizeUpperBound_PerSyncInterval = support8.syncAttribute_VelocityQuantizeUpperBound * support8_effectiveSyncInterval;
			support8.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support8.codeGenerationMemberType = GONetSyncableValueTypes.UnityEngine_Vector2;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 8), out support8.syncAttribute_ShouldSkipSync);
			support8.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1f, 1f, 0, true);

			cachedCustomSerializers[8] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector2Serializer>(0, -1f, 1f);
			cachedValueSerializers[8] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector2Serializer>(0, -1f, 1f);
			cachedVelocitySerializers[8] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector2Serializer>(18, -20.000000f, 20.000000f);

			cachedCustomValueBlendings[8] = GONetAutoMagicalSyncAttribute.GetCustomValueBlending<GONet.PluginAPI.GONetDefaultValueBlending_Vector2>();
			cachedCustomVelocityBlendings[8] = GONet.Utils.ValueBlendUtils.GetDefaultVelocityBlending(support8.lastKnownValue.GONetSyncType);
            int support8_mostRecentChanges_calcdSize = support8.syncAttribute_SyncChangesEverySeconds != 0 ? (int)((GONetMain.valueBlendingBufferLeadSeconds / support8.syncAttribute_SyncChangesEverySeconds) * 2.5f) : 0;
            support8.mostRecentChanges_capacitySize = Math.Max(support8_mostRecentChanges_calcdSize, GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.MOST_RECENT_CHANGEs_SIZE_MINIMUM);
			support8.mostRecentChanges = GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.mostRecentChangesPool.Borrow(support8.mostRecentChanges_capacitySize);

			var support9 = valuesChangesSupport[9] = valueChangeSupportArrayPool.Borrow();
            support9.baselineValue_current.System_UInt32 = EntityStateSynchronizer.targetEntityId; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support9.lastKnownValue.System_UInt32 = EntityStateSynchronizer.targetEntityId; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support9.lastKnownValue_previous.System_UInt32 = EntityStateSynchronizer.targetEntityId; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support9.valueLimitEncountered_min.System_UInt32 = EntityStateSynchronizer.targetEntityId; 
			support9.valueLimitEncountered_max.System_UInt32 = EntityStateSynchronizer.targetEntityId; 
			support9.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support9.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support9.syncCompanion = this;
			support9.memberName = "targetEntityId";
			support9.index = 9;
			support9.syncAttribute_MustRunOnUnityMainThread = true;
			support9.syncAttribute_ProcessingPriority = 0;
			support9.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support9.syncAttribute_SyncChangesEverySeconds = 0.02f;
			support9.syncAttribute_Reliability = AutoMagicalSyncReliability.Reliable;
			support9.syncAttribute_ShouldBlendBetweenValuesReceived = false;
			support9.syncAttribute_PhysicsUpdateInterval = 1;
			support9.syncAttribute_EnablePhysicsSnapping = false;
			support9.isVelocityEligible = false;
			support9.syncAttribute_VelocityQuantizeLowerBound = -20.000000f;
			support9.syncAttribute_VelocityQuantizeUpperBound = 20.000000f;
			support9.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support9_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support9_effectiveSyncInterval = support9_isPhysicsObject && support9.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support9.syncAttribute_PhysicsUpdateInterval
				: support9.syncAttribute_SyncChangesEverySeconds;
			support9.velocityQuantizeLowerBound_PerSyncInterval = support9.syncAttribute_VelocityQuantizeLowerBound * support9_effectiveSyncInterval;
			support9.velocityQuantizeUpperBound_PerSyncInterval = support9.syncAttribute_VelocityQuantizeUpperBound * support9_effectiveSyncInterval;
			support9.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support9.codeGenerationMemberType = GONetSyncableValueTypes.System_UInt32;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 9), out support9.syncAttribute_ShouldSkipSync);
			support9.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);


			var support10 = valuesChangesSupport[10] = valueChangeSupportArrayPool.Borrow();
            support10.baselineValue_current.UnityEngine_Vector3 = EntityStateSynchronizer.velocityVector; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support10.lastKnownValue.UnityEngine_Vector3 = EntityStateSynchronizer.velocityVector; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support10.lastKnownValue_previous.UnityEngine_Vector3 = EntityStateSynchronizer.velocityVector; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support10.valueLimitEncountered_min.UnityEngine_Vector3 = EntityStateSynchronizer.velocityVector; 
			support10.valueLimitEncountered_max.UnityEngine_Vector3 = EntityStateSynchronizer.velocityVector; 
			support10.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support10.lastKnownValue_previous_elapsedTicks = GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support10.syncCompanion = this;
			support10.memberName = "velocityVector";
			support10.index = 10;
			support10.syncAttribute_MustRunOnUnityMainThread = true;
			support10.syncAttribute_ProcessingPriority = 0;
			support10.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support10.syncAttribute_SyncChangesEverySeconds = 0.03125f;
			support10.syncAttribute_Reliability = AutoMagicalSyncReliability.Unreliable;
			support10.syncAttribute_ShouldBlendBetweenValuesReceived = true;
			support10.syncAttribute_PhysicsUpdateInterval = 1;
			support10.syncAttribute_EnablePhysicsSnapping = false;
			support10.isVelocityEligible = false;
			support10.syncAttribute_VelocityQuantizeLowerBound = -0.009766f;
			support10.syncAttribute_VelocityQuantizeUpperBound = 0.009766f;
			support10.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support10_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support10_effectiveSyncInterval = support10_isPhysicsObject && support10.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support10.syncAttribute_PhysicsUpdateInterval
				: support10.syncAttribute_SyncChangesEverySeconds;
			support10.velocityQuantizeLowerBound_PerSyncInterval = support10.syncAttribute_VelocityQuantizeLowerBound * support10_effectiveSyncInterval;
			support10.velocityQuantizeUpperBound_PerSyncInterval = support10.syncAttribute_VelocityQuantizeUpperBound * support10_effectiveSyncInterval;
			support10.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support10.codeGenerationMemberType = GONetSyncableValueTypes.UnityEngine_Vector3;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 10), out support10.syncAttribute_ShouldSkipSync);
			support10.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-10f, 10f, 16, true);

			cachedCustomSerializers[10] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(16, -10f, 10f);
			cachedValueSerializers[10] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(16, -10f, 10f);
			cachedVelocitySerializers[10] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(16, -0.009766f, 0.009766f);

			cachedCustomValueBlendings[10] = GONetAutoMagicalSyncAttribute.GetCustomValueBlending<GONet.PluginAPI.GONetValueBlending_Vector3_ExtrapolateWithLowPassSmoothingFilter>();
			cachedCustomVelocityBlendings[10] = GONet.Utils.ValueBlendUtils.GetDefaultVelocityBlending(support10.lastKnownValue.GONetSyncType);
            int support10_mostRecentChanges_calcdSize = support10.syncAttribute_SyncChangesEverySeconds != 0 ? (int)((GONetMain.valueBlendingBufferLeadSeconds / support10.syncAttribute_SyncChangesEverySeconds) * 2.5f) : 0;
            support10.mostRecentChanges_capacitySize = Math.Max(support10_mostRecentChanges_calcdSize, GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.MOST_RECENT_CHANGEs_SIZE_MINIMUM);
			support10.mostRecentChanges = GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.mostRecentChangesPool.Borrow(support10.mostRecentChanges_capacitySize);

			var support11 = valuesChangesSupport[11] = valueChangeSupportArrayPool.Borrow();
            support11.baselineValue_current.UnityEngine_Quaternion = Transform.rotation; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support11.lastKnownValue.UnityEngine_Quaternion = Transform.rotation; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support11.lastKnownValue_previous.UnityEngine_Quaternion = Transform.rotation; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support11.valueLimitEncountered_min.UnityEngine_Quaternion = Transform.rotation; 
			support11.valueLimitEncountered_max.UnityEngine_Quaternion = Transform.rotation; 
			support11.lastKnownValue_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support11.lastKnownValue_previous_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support11.syncCompanion = this;
			support11.memberName = "rotation";
			support11.index = 11;
			support11.syncAttribute_MustRunOnUnityMainThread = true;
			support11.syncAttribute_ProcessingPriority = 0;
			support11.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support11.syncAttribute_SyncChangesEverySeconds = 0.04166667f;
			support11.syncAttribute_Reliability = AutoMagicalSyncReliability.Unreliable;
			support11.syncAttribute_ShouldBlendBetweenValuesReceived = true;
			support11.syncAttribute_PhysicsUpdateInterval = 1;
			support11.syncAttribute_EnablePhysicsSnapping = false;
			support11.isVelocityEligible = true;
			support11.syncAttribute_VelocityQuantizeLowerBound = -0.133614f;
			support11.syncAttribute_VelocityQuantizeUpperBound = 0.133614f;
			support11.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support11_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support11_effectiveSyncInterval = support11_isPhysicsObject && support11.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support11.syncAttribute_PhysicsUpdateInterval
				: support11.syncAttribute_SyncChangesEverySeconds;
			support11.velocityQuantizeLowerBound_PerSyncInterval = support11.syncAttribute_VelocityQuantizeLowerBound * support11_effectiveSyncInterval;
			support11.velocityQuantizeUpperBound_PerSyncInterval = support11.syncAttribute_VelocityQuantizeUpperBound * support11_effectiveSyncInterval;
			support11.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support11.codeGenerationMemberType = GONetSyncableValueTypes.UnityEngine_Quaternion;
			support11.syncAttribute_ShouldSkipSync = GONetMain.IsRotationNotSyncd;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 11), out support11.syncAttribute_ShouldSkipSync);
			support11.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-1.701412E+38f, 1.701412E+38f, 0, true);

			cachedCustomSerializers[11] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.QuaternionSerializer>(0, -1.701412E+38f, 1.701412E+38f);
			cachedValueSerializers[11] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.QuaternionSerializer>(0, -1.701412E+38f, 1.701412E+38f);
			cachedVelocitySerializers[11] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(9, -0.133614f, 0.133614f);

			cachedCustomValueBlendings[11] = GONetAutoMagicalSyncAttribute.GetCustomValueBlending<GONet.PluginAPI.GONetValueBlending_Quaternion_ExtrapolateWithLowPassSmoothingFilter>();
			cachedCustomVelocityBlendings[11] = GONet.Utils.ValueBlendUtils.GetDefaultVelocityBlending(support11.lastKnownValue.GONetSyncType);
            int support11_mostRecentChanges_calcdSize = support11.syncAttribute_SyncChangesEverySeconds != 0 ? (int)((GONetMain.valueBlendingBufferLeadSeconds / support11.syncAttribute_SyncChangesEverySeconds) * 2.5f) : 0;
            support11.mostRecentChanges_capacitySize = Math.Max(support11_mostRecentChanges_calcdSize, GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.MOST_RECENT_CHANGEs_SIZE_MINIMUM);
			support11.mostRecentChanges = GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.mostRecentChangesPool.Borrow(support11.mostRecentChanges_capacitySize);

			var support12 = valuesChangesSupport[12] = valueChangeSupportArrayPool.Borrow();
            support12.baselineValue_current.UnityEngine_Vector3 = Transform.position; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support12.lastKnownValue.UnityEngine_Vector3 = Transform.position; // IMPORTANT: The use of the property here (i.e., prior to use anywhere herein after) ensures GetComponnet<T>() called up front and that component is cached and available subsequently as needed/referenced/used
            support12.lastKnownValue_previous.UnityEngine_Vector3 = Transform.position; // IMPORTANT: same as above PLUS capturing the initial value now as the previous will ensure we do not accumulate changes during first pass "has anything changed" checks, which caused some problems before putting this in because things run in different threads and this is appropriate!
			support12.valueLimitEncountered_min.UnityEngine_Vector3 = Transform.position; 
			support12.valueLimitEncountered_max.UnityEngine_Vector3 = Transform.position; 
			support12.lastKnownValue_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks; // Initialize current timestamp
			support12.lastKnownValue_previous_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks; // Initialize previous timestamp (same as current at initialization)
			support12.syncCompanion = this;
			support12.memberName = "position";
			support12.index = 12;
			support12.syncAttribute_MustRunOnUnityMainThread = true;
			support12.syncAttribute_ProcessingPriority = 0;
			support12.syncAttribute_ProcessingPriority_GONetInternalOverride = 0;
			support12.syncAttribute_SyncChangesEverySeconds = 0.04166667f;
			support12.syncAttribute_Reliability = AutoMagicalSyncReliability.Unreliable;
			support12.syncAttribute_ShouldBlendBetweenValuesReceived = true;
			support12.syncAttribute_PhysicsUpdateInterval = 1;
			support12.syncAttribute_EnablePhysicsSnapping = false;
			support12.isVelocityEligible = true;
			support12.syncAttribute_VelocityQuantizeLowerBound = -0.366233f;
			support12.syncAttribute_VelocityQuantizeUpperBound = 0.366233f;
			support12.syncAttribute_VelocityAnchorIntervalSeconds = 0.0f;
			// Runtime detection: Use physics-gated interval for physics objects, configured interval otherwise
			bool support12_isPhysicsObject = gonetParticipant.IsRigidBodyOwnerOnlyControlled && gonetParticipant.myRigidBody != null;
			float support12_effectiveSyncInterval = support12_isPhysicsObject && support12.syncAttribute_PhysicsUpdateInterval > 0
				? UnityEngine.Time.fixedDeltaTime * support12.syncAttribute_PhysicsUpdateInterval
				: support12.syncAttribute_SyncChangesEverySeconds;
			support12.velocityQuantizeLowerBound_PerSyncInterval = support12.syncAttribute_VelocityQuantizeLowerBound * support12_effectiveSyncInterval;
			support12.velocityQuantizeUpperBound_PerSyncInterval = support12.syncAttribute_VelocityQuantizeUpperBound * support12_effectiveSyncInterval;
			support12.lastVelocityTimestamp = GONet.GONetMain.Time.ElapsedTicks;
			support12.codeGenerationMemberType = GONetSyncableValueTypes.UnityEngine_Vector3;
			support12.syncAttribute_ShouldSkipSync = GONetMain.IsPositionNotSyncd;
			// TODO have to revisit this at some point to get animator parameters working: GONet.GONetAutoMagicalSyncAttribute.ShouldSkipSyncByRegistrationIdMap.TryGetValue((5, 12), out support12.syncAttribute_ShouldSkipSync);
			support12.syncAttribute_QuantizerSettingsGroup = new GONet.Utils.QuantizerSettingsGroup(-125f, 125f, 14, true);

			cachedCustomSerializers[12] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(14, -125f, 125f);
			cachedValueSerializers[12] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(14, -125f, 125f);
			cachedVelocitySerializers[12] = GONetAutoMagicalSyncAttribute.GetCustomSerializer<GONet.Vector3Serializer>(14, -0.366233f, 0.366233f);

			cachedCustomValueBlendings[12] = GONetAutoMagicalSyncAttribute.GetCustomValueBlending<GONet.PluginAPI.GONetValueBlending_Vector3_HermiteSpline>();
			cachedCustomVelocityBlendings[12] = GONet.Utils.ValueBlendUtils.GetDefaultVelocityBlending(support12.lastKnownValue.GONetSyncType);
            int support12_mostRecentChanges_calcdSize = support12.syncAttribute_SyncChangesEverySeconds != 0 ? (int)((GONetMain.valueBlendingBufferLeadSeconds / support12.syncAttribute_SyncChangesEverySeconds) * 2.5f) : 0;
            support12.mostRecentChanges_capacitySize = Math.Max(support12_mostRecentChanges_calcdSize, GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.MOST_RECENT_CHANGEs_SIZE_MINIMUM);
			support12.mostRecentChanges = GONetMain.AutoMagicalSync_ValueMonitoringSupport_ChangedValue.mostRecentChangesPool.Borrow(support12.mostRecentChanges_capacitySize);

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
					EntityStateSynchronizer.cameraLookAtEulerAngles = value.UnityEngine_Vector3;
					return;
				case 6:
					EntityStateSynchronizer.isCrouched = value.System_Boolean;
					return;
				case 7:
					EntityStateSynchronizer.isGrounded = value.System_Boolean;
					return;
				case 8:
					EntityStateSynchronizer.movementInput = value.UnityEngine_Vector2;
					return;
				case 9:
					EntityStateSynchronizer.targetEntityId = value.System_UInt32;
					return;
				case 10:
					EntityStateSynchronizer.velocityVector = value.UnityEngine_Vector3;
					return;
				case 11:
					Transform.rotation = value.UnityEngine_Quaternion;
					return;
				case 12:
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
					return EntityStateSynchronizer.cameraLookAtEulerAngles;
				case 6:
					return EntityStateSynchronizer.isCrouched;
				case 7:
					return EntityStateSynchronizer.isGrounded;
				case 8:
					return EntityStateSynchronizer.movementInput;
				case 9:
					return EntityStateSynchronizer.targetEntityId;
				case 10:
					return EntityStateSynchronizer.velocityVector;
				case 11:
					return Transform.rotation;
				case 12:
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
			{ // EntityStateSynchronizer.cameraLookAtEulerAngles
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[5];
				customSerializer.Serialize(bitStream_appendTo, gonetParticipant, EntityStateSynchronizer.cameraLookAtEulerAngles);
			}
			{ // EntityStateSynchronizer.isCrouched
				bitStream_appendTo.WriteBit(EntityStateSynchronizer.isCrouched);
			}
			{ // EntityStateSynchronizer.isGrounded
				bitStream_appendTo.WriteBit(EntityStateSynchronizer.isGrounded);
			}
			{ // EntityStateSynchronizer.movementInput
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[8];
				customSerializer.Serialize(bitStream_appendTo, gonetParticipant, EntityStateSynchronizer.movementInput);
			}
			{ // EntityStateSynchronizer.targetEntityId
				bitStream_appendTo.WriteUInt(EntityStateSynchronizer.targetEntityId);
			}
			{ // EntityStateSynchronizer.velocityVector
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[10];
				{ // SUB-QUANTIZATION DIAGNOSTIC for velocityVector
					var currentValue = EntityStateSynchronizer.velocityVector;
					var baselineValue = valuesChangesSupport[10].baselineValue_current.UnityEngine_Vector3;
					var deltaFromBaseline = currentValue - baselineValue;
					// GONet.Utils.SubQuantizationDiagnostics.CheckAndLogIfSubQuantization(gonetParticipant.GONetId, "velocityVector", deltaFromBaseline, valuesChangesSupport[10].syncAttribute_QuantizerSettingsGroup, customSerializer);
					customSerializer.Serialize(bitStream_appendTo, gonetParticipant, deltaFromBaseline);
				}
			}
			{ // Transform.rotation
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[11];
				customSerializer.Serialize(bitStream_appendTo, gonetParticipant, Transform.rotation);
			}
			{ // Transform.position
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[12];
				{ // SUB-QUANTIZATION DIAGNOSTIC for position
					var currentValue = Transform.position;
					var baselineValue = valuesChangesSupport[12].baselineValue_current.UnityEngine_Vector3;
					var deltaFromBaseline = currentValue - baselineValue;
					// GONet.Utils.SubQuantizationDiagnostics.CheckAndLogIfSubQuantization(gonetParticipant.GONetId, "position", deltaFromBaseline, valuesChangesSupport[12].syncAttribute_QuantizerSettingsGroup, customSerializer);
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
				{ // EntityStateSynchronizer.cameraLookAtEulerAngles
					    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[5];
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
						cachedVelocitySerializers[5].Serialize(bitStream_appendTo, gonetParticipant, velocityValue);
						// VelocitySyncTelemetry.TrackVelocityBundleSent(gonetParticipant.GONetId);
					}
					else
					{
						// VALUE BUNDLE: Serialize position normally
						customSerializer.Serialize(bitStream_appendTo, gonetParticipant, EntityStateSynchronizer.cameraLookAtEulerAngles);
				
						// Store snapshot for velocity calculation on next VELOCITY bundle
						var snapshotValue = new GONetSyncableValue();
						snapshotValue.UnityEngine_Vector3 = EntityStateSynchronizer.cameraLookAtEulerAngles;
						valuesChangesSupport[5].AddToMostRecentChangeQueue_IfAppropriate(GONet.GONetMain.Time.ElapsedTicks, snapshotValue);
						// VelocitySyncTelemetry.TrackValueBundleSent(gonetParticipant.GONetId);
					}
				}
				break;

				case 6:
				{ // EntityStateSynchronizer.isCrouched
					bitStream_appendTo.WriteBit(EntityStateSynchronizer.isCrouched);
				}
				break;

				case 7:
				{ // EntityStateSynchronizer.isGrounded
					bitStream_appendTo.WriteBit(EntityStateSynchronizer.isGrounded);
				}
				break;

				case 8:
				{ // EntityStateSynchronizer.movementInput
					    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[8];
					if (isVelocityBundle)
					{
						// VELOCITY BUNDLE: Send raw delta (no division - optimization!)
						var changesSupport = valuesChangesSupport[8];
						GONetSyncableValue velocityValue;
						
						// CRITICAL: Use lastKnownValue (authority's transform) NOT mostRecentChanges (client blending queue)
						var current = changesSupport.lastKnownValue;
						var previous = changesSupport.lastKnownValue_previous;
						
						// Check if we have valid previous value
						if (current.GONetSyncType == previous.GONetSyncType && current.GONetSyncType != GONet.GONetSyncableValueTypes.System_Boolean)
						{
							// Calculate velocity from position change (units/sec)
							UnityEngine.Vector2 currentValue = current.UnityEngine_Vector2;
							UnityEngine.Vector2 previousValue = previous.UnityEngine_Vector2;
						
							// Calculate deltaTime from timestamps
							long deltaTimeTicks = changesSupport.lastKnownValue_elapsedTicks - changesSupport.lastKnownValue_previous_elapsedTicks;
							float deltaTime = (float)(deltaTimeTicks * GONet.Utils.HighResolutionTimeUtils.TICKS_TO_SECONDS);
							if (deltaTime <= 0f) deltaTime = 0.0167f; // 60 FPS fallback
						
							velocityValue = new GONetSyncableValue();
							UnityEngine.Vector2 deltaValue = currentValue - previousValue;
							velocityValue.UnityEngine_Vector2 = deltaValue / deltaTime;  // Convert to units/sec
							// VelocitySyncTelemetry.TrackVelocityCalculation(true, gonetParticipant.GONetId);
						}
						else
						{
							// Type mismatch, use zero delta
							GONet.GONetLog.Warning($"[VELOCITY-CALC-FALLBACK] GONetId={gonetParticipant.GONetId} currentType={current.GONetSyncType} previousType={previous.GONetSyncType} - Using zero velocity");
							velocityValue = new GONetSyncableValue();
							velocityValue.UnityEngine_Vector2 = UnityEngine.Vector2.zero;
							// VelocitySyncTelemetry.TrackVelocityCalculation(false, gonetParticipant.GONetId);
						}
						
						// Serialize velocity using velocity serializer
						cachedVelocitySerializers[8].Serialize(bitStream_appendTo, gonetParticipant, velocityValue);
						// VelocitySyncTelemetry.TrackVelocityBundleSent(gonetParticipant.GONetId);
					}
					else
					{
						// VALUE BUNDLE: Serialize position normally
						customSerializer.Serialize(bitStream_appendTo, gonetParticipant, EntityStateSynchronizer.movementInput);
				
						// Store snapshot for velocity calculation on next VELOCITY bundle
						var snapshotValue = new GONetSyncableValue();
						snapshotValue.UnityEngine_Vector2 = EntityStateSynchronizer.movementInput;
						valuesChangesSupport[8].AddToMostRecentChangeQueue_IfAppropriate(GONet.GONetMain.Time.ElapsedTicks, snapshotValue);
						// VelocitySyncTelemetry.TrackValueBundleSent(gonetParticipant.GONetId);
					}
				}
				break;

				case 9:
				{ // EntityStateSynchronizer.targetEntityId
					bitStream_appendTo.WriteUInt(EntityStateSynchronizer.targetEntityId);
				}
				break;

				case 10:
				{ // EntityStateSynchronizer.velocityVector
					    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[10];
					if (isVelocityBundle)
					{
						// VELOCITY BUNDLE: Send raw delta (no division - optimization!)
						var changesSupport = valuesChangesSupport[10];
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
						cachedVelocitySerializers[10].Serialize(bitStream_appendTo, gonetParticipant, velocityValue);
						// VelocitySyncTelemetry.TrackVelocityBundleSent(gonetParticipant.GONetId);
					}
					else
					{
						// VALUE BUNDLE: Serialize position normally
						{ // SUB-QUANTIZATION DIAGNOSTIC for velocityVector
							var currentValue = EntityStateSynchronizer.velocityVector;
							var baselineValue = valuesChangesSupport[10].baselineValue_current.UnityEngine_Vector3;
							var deltaFromBaseline = currentValue - baselineValue;
							// GONet.Utils.SubQuantizationDiagnostics.CheckAndLogIfSubQuantization(gonetParticipant.GONetId, "velocityVector", deltaFromBaseline, valuesChangesSupport[10].syncAttribute_QuantizerSettingsGroup, customSerializer);
							customSerializer.Serialize(bitStream_appendTo, gonetParticipant, deltaFromBaseline);
						}
				
						// Store snapshot for velocity calculation on next VELOCITY bundle
						var snapshotValue = new GONetSyncableValue();
						snapshotValue.UnityEngine_Vector3 = EntityStateSynchronizer.velocityVector;
						valuesChangesSupport[10].AddToMostRecentChangeQueue_IfAppropriate(GONet.GONetMain.Time.ElapsedTicks, snapshotValue);
						// VelocitySyncTelemetry.TrackValueBundleSent(gonetParticipant.GONetId);
					}
				}
				break;

				case 11:
				{ // Transform.rotation
					    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[11];
					// VelocitySyncTelemetry.TraceServerAuthority(gonetParticipant.GONetId, 11, "rotation", Transform.rotation);
					if (isVelocityBundle)
					{
						// VELOCITY BUNDLE: Send raw delta (no division - optimization!)
						var changesSupport = valuesChangesSupport[11];
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
						cachedVelocitySerializers[11].Serialize(bitStream_appendTo, gonetParticipant, velocityValue);
						// VelocitySyncTelemetry.TrackVelocityBundleSent(gonetParticipant.GONetId);
						// VelocitySyncTelemetry.TraceServerSend(gonetParticipant.GONetId, 11, "VELOCITY", Transform.rotation, velocityValue.UnityEngine_Vector3, "partition_decision", valuesChangesSupport[11].mostRecentChanges_usedSize, true);
					}
					else
					{
						// VALUE BUNDLE: Serialize position normally
						customSerializer.Serialize(bitStream_appendTo, gonetParticipant, Transform.rotation);
				
						// Store snapshot for velocity calculation on next VELOCITY bundle
						var snapshotValue = new GONetSyncableValue();
						snapshotValue.UnityEngine_Quaternion = Transform.rotation;
						valuesChangesSupport[11].AddToMostRecentChangeQueue_IfAppropriate(GONet.GONetMain.Time.ElapsedTicks, snapshotValue);
						// VelocitySyncTelemetry.TrackValueBundleSent(gonetParticipant.GONetId);
						// VelocitySyncTelemetry.TraceServerSend(gonetParticipant.GONetId, 11, "VALUE", Transform.rotation, UnityEngine.Vector3.zero, "partition_decision", valuesChangesSupport[11].mostRecentChanges_usedSize, false);
					}
				}
				break;

				case 12:
				{ // Transform.position
					    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[12];
					if (isVelocityBundle)
					{
						// VELOCITY BUNDLE: Send raw delta (no division - optimization!)
						var changesSupport = valuesChangesSupport[12];
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
						cachedVelocitySerializers[12].Serialize(bitStream_appendTo, gonetParticipant, velocityValue);
						// VelocitySyncTelemetry.TrackVelocityBundleSent(gonetParticipant.GONetId);
					}
					else
					{
						// VALUE BUNDLE: Serialize position normally
						{ // SUB-QUANTIZATION DIAGNOSTIC for position
							var currentValue = Transform.position;
							var baselineValue = valuesChangesSupport[12].baselineValue_current.UnityEngine_Vector3;
							var deltaFromBaseline = currentValue - baselineValue;
							// GONet.Utils.SubQuantizationDiagnostics.CheckAndLogIfSubQuantization(gonetParticipant.GONetId, "position", deltaFromBaseline, valuesChangesSupport[12].syncAttribute_QuantizerSettingsGroup, customSerializer);
							customSerializer.Serialize(bitStream_appendTo, gonetParticipant, deltaFromBaseline);
						}
				
						// Store snapshot for velocity calculation on next VELOCITY bundle
						var snapshotValue = new GONetSyncableValue();
						snapshotValue.UnityEngine_Vector3 = Transform.position;
						valuesChangesSupport[12].AddToMostRecentChangeQueue_IfAppropriate(GONet.GONetMain.Time.ElapsedTicks, snapshotValue);
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
				{ // EntityStateSynchronizer.cameraLookAtEulerAngles
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[5];
					return customSerializer.AreEqualConsideringQuantization(valueA, valueB);
				}
				break;

				case 6:
				{ // EntityStateSynchronizer.isCrouched
					return valueA.System_Boolean == valueB.System_Boolean;
				}
				break;

				case 7:
				{ // EntityStateSynchronizer.isGrounded
					return valueA.System_Boolean == valueB.System_Boolean;
				}
				break;

				case 8:
				{ // EntityStateSynchronizer.movementInput
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[8];
					return customSerializer.AreEqualConsideringQuantization(valueA, valueB);
				}
				break;

				case 9:
				{ // EntityStateSynchronizer.targetEntityId
					// handle quantization of this type eventually?
				}
				break;

				case 10:
				{ // EntityStateSynchronizer.velocityVector
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[10];
					return customSerializer.AreEqualConsideringQuantization(valueA, valueB);
				}
				break;

				case 11:
				{ // Transform.rotation
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[11];
					return customSerializer.AreEqualConsideringQuantization(valueA, valueB);
				}
				break;

				case 12:
				{ // Transform.position
				    IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[12];
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
			{ // EntityStateSynchronizer.cameraLookAtEulerAngles
				IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[5];
				var value = customSerializer.Deserialize(bitStream_readFrom).UnityEngine_Vector3;
				EntityStateSynchronizer.cameraLookAtEulerAngles = value;
			}
			{ // EntityStateSynchronizer.isCrouched
				bool value;
			                bitStream_readFrom.ReadBit(out value);
				EntityStateSynchronizer.isCrouched = value;
			}
			{ // EntityStateSynchronizer.isGrounded
				bool value;
			                bitStream_readFrom.ReadBit(out value);
				EntityStateSynchronizer.isGrounded = value;
			}
			{ // EntityStateSynchronizer.movementInput
				IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[8];
				var value = customSerializer.Deserialize(bitStream_readFrom).UnityEngine_Vector2;
				EntityStateSynchronizer.movementInput = value;
			}
			{ // EntityStateSynchronizer.targetEntityId
				uint value;
			                bitStream_readFrom.ReadUInt(out value);
				EntityStateSynchronizer.targetEntityId = value;
			}
			{ // EntityStateSynchronizer.velocityVector
				IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[10];
				var value = customSerializer.Deserialize(bitStream_readFrom).UnityEngine_Vector3;
				value += valuesChangesSupport[10].baselineValue_current.UnityEngine_Vector3;
				EntityStateSynchronizer.velocityVector = value;
			}
			{ // Transform.rotation
				IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[11];
				var value = customSerializer.Deserialize(bitStream_readFrom).UnityEngine_Quaternion;
				Transform.rotation = value;
			}
			{ // Transform.position
				IGONetAutoMagicalSync_CustomSerializer customSerializer = cachedCustomSerializers[12];
				var value = customSerializer.Deserialize(bitStream_readFrom).UnityEngine_Vector3;
				value += valuesChangesSupport[12].baselineValue_current.UnityEngine_Vector3;
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
				{ // EntityStateSynchronizer.cameraLookAtEulerAngles
					// Velocity-augmented sync: Choose serializer based on packet type (VALUE vs VELOCITY)
					IGONetAutoMagicalSync_CustomSerializer customSerializer = useVelocitySerializer ? cachedVelocitySerializers[5] : cachedValueSerializers[5];
					GONetSyncableValue value = customSerializer.Deserialize(bitStream_readFrom);
					var typedValue = value.UnityEngine_Vector3;
					value.UnityEngine_Vector3 = typedValue;
					return value;
				}
				case 6:
				{ // EntityStateSynchronizer.isCrouched
					bool value;
				                bitStream_readFrom.ReadBit(out value);
					return value;
				}
				case 7:
				{ // EntityStateSynchronizer.isGrounded
					bool value;
				                bitStream_readFrom.ReadBit(out value);
					return value;
				}
				case 8:
				{ // EntityStateSynchronizer.movementInput
					// Velocity-augmented sync: Choose serializer based on packet type (VALUE vs VELOCITY)
					IGONetAutoMagicalSync_CustomSerializer customSerializer = useVelocitySerializer ? cachedVelocitySerializers[8] : cachedValueSerializers[8];
					GONetSyncableValue value = customSerializer.Deserialize(bitStream_readFrom);
					var typedValue = value.UnityEngine_Vector2;
					value.UnityEngine_Vector2 = typedValue;
					return value;
				}
				case 9:
				{ // EntityStateSynchronizer.targetEntityId
					uint value;
				                bitStream_readFrom.ReadUInt(out value);
					return value;
				}
				case 10:
				{ // EntityStateSynchronizer.velocityVector
					// Velocity-augmented sync: Choose serializer based on packet type (VALUE vs VELOCITY)
					IGONetAutoMagicalSync_CustomSerializer customSerializer = useVelocitySerializer ? cachedVelocitySerializers[10] : cachedValueSerializers[10];
					GONetSyncableValue value = customSerializer.Deserialize(bitStream_readFrom);
					var typedValue = value.UnityEngine_Vector3;
					if (!useVelocitySerializer)
					{
						typedValue += valuesChangesSupport[10].baselineValue_current.UnityEngine_Vector3;
					}
					value.UnityEngine_Vector3 = typedValue;
					return value;
				}
				case 11:
				{ // Transform.rotation
					// Velocity-augmented sync: Choose serializer based on packet type (VALUE vs VELOCITY)
					IGONetAutoMagicalSync_CustomSerializer customSerializer = useVelocitySerializer ? cachedVelocitySerializers[11] : cachedValueSerializers[11];
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
				case 12:
				{ // Transform.position
					// Velocity-augmented sync: Choose serializer based on packet type (VALUE vs VELOCITY)
					IGONetAutoMagicalSync_CustomSerializer customSerializer = useVelocitySerializer ? cachedVelocitySerializers[12] : cachedValueSerializers[12];
					GONetSyncableValue value = customSerializer.Deserialize(bitStream_readFrom);
					var typedValue = value.UnityEngine_Vector3;
					if (!useVelocitySerializer)
					{
						typedValue += valuesChangesSupport[12].baselineValue_current.UnityEngine_Vector3;
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
				case 6:					EntityStateSynchronizer.isCrouched = value.System_Boolean; break;
				case 7:					EntityStateSynchronizer.isGrounded = value.System_Boolean; break;
				case 8:					valuesChangesSupport[8].AddToMostRecentChangeQueue_IfAppropriate(assumedElapsedTicksAtChange, value); break; // NOTE: this queue will be used each frame to blend between this value and others added there
				case 9:					EntityStateSynchronizer.targetEntityId = value.System_UInt32; break;
				case 10:					valuesChangesSupport[10].AddToMostRecentChangeQueue_IfAppropriate(assumedElapsedTicksAtChange, value); break; // NOTE: this queue will be used each frame to blend between this value and others added there
				case 11:					valuesChangesSupport[11].AddToMostRecentChangeQueue_IfAppropriate(assumedElapsedTicksAtChange, value); break; // NOTE: this queue will be used each frame to blend between this value and others added there
				case 12:					valuesChangesSupport[12].AddToMostRecentChangeQueue_IfAppropriate(assumedElapsedTicksAtChange, value); break; // NOTE: this queue will be used each frame to blend between this value and others added there
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
					valuesChangesSupport5.lastKnownValue.UnityEngine_Vector3 = EntityStateSynchronizer.cameraLookAtEulerAngles;
					valuesChangesSupport5.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport6 = valuesChangesSupport[6];
				if (DoesMatchUniqueGrouping(valuesChangesSupport6, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport6, 6)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					valuesChangesSupport6.lastKnownValue.System_Boolean = EntityStateSynchronizer.isCrouched;
					valuesChangesSupport6.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport7 = valuesChangesSupport[7];
				if (DoesMatchUniqueGrouping(valuesChangesSupport7, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport7, 7)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					valuesChangesSupport7.lastKnownValue.System_Boolean = EntityStateSynchronizer.isGrounded;
					valuesChangesSupport7.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport8 = valuesChangesSupport[8];
				if (DoesMatchUniqueGrouping(valuesChangesSupport8, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport8, 8)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					valuesChangesSupport8.lastKnownValue.UnityEngine_Vector2 = EntityStateSynchronizer.movementInput;
					valuesChangesSupport8.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport9 = valuesChangesSupport[9];
				if (DoesMatchUniqueGrouping(valuesChangesSupport9, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport9, 9)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					valuesChangesSupport9.lastKnownValue.System_UInt32 = EntityStateSynchronizer.targetEntityId;
					valuesChangesSupport9.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport10 = valuesChangesSupport[10];
				if (DoesMatchUniqueGrouping(valuesChangesSupport10, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport10, 10)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					valuesChangesSupport10.lastKnownValue.UnityEngine_Vector3 = EntityStateSynchronizer.velocityVector;
					valuesChangesSupport10.lastKnownValue_elapsedTicks = GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport11 = valuesChangesSupport[11];
				if (DoesMatchUniqueGrouping(valuesChangesSupport11, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport11, 11)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					// Velocity-eligible: Save OLD current to previous BEFORE updating current
					valuesChangesSupport11.lastKnownValue_previous_elapsedTicks = valuesChangesSupport11.lastKnownValue_elapsedTicks;
					valuesChangesSupport11.lastKnownValue_previous = valuesChangesSupport11.lastKnownValue;

					if (shouldSourceFromRigidbody)
					{
						// Source from Rigidbody (physics simulation)
						valuesChangesSupport11.lastKnownValue.UnityEngine_Quaternion = gonetParticipant.myRigidBody.rotation;
					}
					else
					{
						// Source from Transform (regular sync)
						valuesChangesSupport11.lastKnownValue.UnityEngine_Quaternion = Transform.rotation;
					}
					valuesChangesSupport11.lastKnownValue_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks;
				}

				var valuesChangesSupport12 = valuesChangesSupport[12];
				if (DoesMatchUniqueGrouping(valuesChangesSupport12, onlyMatchIfUniqueGroupingMatches) &&
					!ShouldSkipSync(valuesChangesSupport12, 12)) // TODO examine eval order and performance...should this be first or last?, TODO also consider taking this check out of this condition alltogether, because it is perhaps more expensive to do this check than it is to just execute the body AND the body execution will not actually affect whether or not this value change will get sync'd or not..hmm...
				{
					// Velocity-eligible: Save OLD current to previous BEFORE updating current
					valuesChangesSupport12.lastKnownValue_previous_elapsedTicks = valuesChangesSupport12.lastKnownValue_elapsedTicks;
					valuesChangesSupport12.lastKnownValue_previous = valuesChangesSupport12.lastKnownValue;

					if (shouldSourceFromRigidbody)
					{
						// Source from Rigidbody (physics simulation)
						valuesChangesSupport12.lastKnownValue.UnityEngine_Vector3 = gonetParticipant.myRigidBody.position;
					}
					else
					{
						// Source from Transform (regular sync)
						valuesChangesSupport12.lastKnownValue.UnityEngine_Vector3 = Transform.position;
					}
					valuesChangesSupport12.lastKnownValue_elapsedTicks = shouldSourceFromRigidbody ? GONetMain.Time.FixedElapsedTicks : GONetMain.Time.ElapsedTicks;
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
				{ // EntityStateSynchronizer.cameraLookAtEulerAngles
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

				case 6:
				{ // EntityStateSynchronizer.isCrouched
					// this type not supported for this functionality
				}
				break;

				case 7:
				{ // EntityStateSynchronizer.isGrounded
					// this type not supported for this functionality
				}
				break;

				case 8:
				{ // EntityStateSynchronizer.movementInput
					// this type not supported for this functionality
				}
				break;

				case 9:
				{ // EntityStateSynchronizer.targetEntityId
					// this type not supported for this functionality
				}
				break;

				case 10:
				{ // EntityStateSynchronizer.velocityVector
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

				case 11:
				{ // Transform.rotation
					// this type not supported for this functionality
				}
				break;

				case 12:
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
				{ // EntityStateSynchronizer.cameraLookAtEulerAngles
					return new ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector3() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.UnityEngine_Vector3
					};
				}

				case 6:
				{ // EntityStateSynchronizer.isCrouched
					return new ValueMonitoringSupport_NewBaselineEvent_System_Boolean() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.System_Boolean
					};
				}

				case 7:
				{ // EntityStateSynchronizer.isGrounded
					return new ValueMonitoringSupport_NewBaselineEvent_System_Boolean() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.System_Boolean
					};
				}

				case 8:
				{ // EntityStateSynchronizer.movementInput
					return new ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector2() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.UnityEngine_Vector2
					};
				}

				case 9:
				{ // EntityStateSynchronizer.targetEntityId
					return new ValueMonitoringSupport_NewBaselineEvent_System_UInt32() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.System_UInt32
					};
				}

				case 10:
				{ // EntityStateSynchronizer.velocityVector
					return new ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Vector3() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.UnityEngine_Vector3
					};
				}

				case 11:
				{ // Transform.rotation
					return new ValueMonitoringSupport_NewBaselineEvent_UnityEngine_Quaternion() {
						GONetId = gonetId,
						ValueIndex = singleIndex,
						NewBaselineValue = newBaselineValue.UnityEngine_Quaternion
					};
				}

				case 12:
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
