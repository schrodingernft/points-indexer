// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: points_contract.proto
// </auto-generated>
// Original file comments:
// the version of the language, use proto3 for contracts
#pragma warning disable 0414, 1591
#region Designer generated code

using System.Collections.Generic;
using aelf = global::AElf.CSharp.Core;

namespace Points.Contracts.Point {

  #region Events

  public partial class Joined : aelf::IEvent<Joined>
  {
    public global::System.Collections.Generic.IEnumerable<Joined> GetIndexed()
    {
      return new List<Joined>
      {
      };
    }

    public Joined GetNonIndexed()
    {
      return new Joined
      {
        DappId = DappId,
        Domain = Domain,
        Registrant = Registrant,
      };
    }
  }

  public partial class PointsUpdated : aelf::IEvent<PointsUpdated>
  {
    public global::System.Collections.Generic.IEnumerable<PointsUpdated> GetIndexed()
    {
      return new List<PointsUpdated>
      {
      };
    }

    public PointsUpdated GetNonIndexed()
    {
      return new PointsUpdated
      {
        PointStateList = PointStateList,
      };
    }
  }

  internal partial class Registered : aelf::IEvent<Registered>
  {
    public global::System.Collections.Generic.IEnumerable<Registered> GetIndexed()
    {
      return new List<Registered>
      {
      };
    }

    public Registered GetNonIndexed()
    {
      return new Registered
      {
        RegistrationRecordList = RegistrationRecordList,
      };
    }
  }

  internal partial class ServicesEarningRulesChanged : aelf::IEvent<ServicesEarningRulesChanged>
  {
    public global::System.Collections.Generic.IEnumerable<ServicesEarningRulesChanged> GetIndexed()
    {
      return new List<ServicesEarningRulesChanged>
      {
      };
    }

    public ServicesEarningRulesChanged GetNonIndexed()
    {
      return new ServicesEarningRulesChanged
      {
        Service = Service,
        ServicesEarningRules = ServicesEarningRules,
      };
    }
  }

  internal partial class PointCreated : aelf::IEvent<PointCreated>
  {
    public global::System.Collections.Generic.IEnumerable<PointCreated> GetIndexed()
    {
      return new List<PointCreated>
      {
      };
    }

    public PointCreated GetNonIndexed()
    {
      return new PointCreated
      {
        TokenName = TokenName,
        Decimals = Decimals,
      };
    }
  }

  public partial class InviterApplied : aelf::IEvent<InviterApplied>
  {
    public global::System.Collections.Generic.IEnumerable<InviterApplied> GetIndexed()
    {
      return new List<InviterApplied>
      {
      };
    }

    public InviterApplied GetNonIndexed()
    {
      return new InviterApplied
      {
        Domain = Domain,
        DappId = DappId,
        Invitee = Invitee,
        Inviter = Inviter,
      };
    }
  }

  public partial class PointsRecorded : aelf::IEvent<PointsRecorded>
  {
    public global::System.Collections.Generic.IEnumerable<PointsRecorded> GetIndexed()
    {
      return new List<PointsRecorded>
      {
      };
    }

    public PointsRecorded GetNonIndexed()
    {
      return new PointsRecorded
      {
        PointsRecordList = PointsRecordList,
      };
    }
  }

  #endregion
  internal static partial class PointsContractContainer
  {
    static readonly string __ServiceName = "PointsContract";

    #region Marshallers
    static readonly aelf::Marshaller<global::Points.Contracts.Point.InitializeInput> __Marshaller_InitializeInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.InitializeInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Google.Protobuf.WellKnownTypes.Empty> __Marshaller_google_protobuf_Empty = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Empty.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::AElf.Types.Address> __Marshaller_aelf_Address = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::AElf.Types.Address.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.CreatePointInput> __Marshaller_CreatePointInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.CreatePointInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.JoinInput> __Marshaller_JoinInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.JoinInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.SettleInput> __Marshaller_SettleInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.SettleInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.RecordRegistrationInput> __Marshaller_RecordRegistrationInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.RecordRegistrationInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.ApplyToOperatorInput> __Marshaller_ApplyToOperatorInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.ApplyToOperatorInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.PointsSettlementInput> __Marshaller_PointsSettlementInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.PointsSettlementInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Google.Protobuf.WellKnownTypes.Int32Value> __Marshaller_google_protobuf_Int32Value = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.Int32Value.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.SetReservedDomainListInput> __Marshaller_SetReservedDomainListInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.SetReservedDomainListInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.GetReservedDomainListOutput> __Marshaller_GetReservedDomainListOutput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.GetReservedDomainListOutput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.SetServicesEarningRulesInput> __Marshaller_SetServicesEarningRulesInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.SetServicesEarningRulesInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.GetServicesEarningRulesInput> __Marshaller_GetServicesEarningRulesInput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.GetServicesEarningRulesInput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.GetServicesEarningRulesOutput> __Marshaller_GetServicesEarningRulesOutput = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.GetServicesEarningRulesOutput.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Google.Protobuf.WellKnownTypes.StringValue> __Marshaller_google_protobuf_StringValue = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Protobuf.WellKnownTypes.StringValue.Parser.ParseFrom);
    static readonly aelf::Marshaller<global::Points.Contracts.Point.DomainOperatorRelationship> __Marshaller_DomainOperatorRelationship = aelf::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Points.Contracts.Point.DomainOperatorRelationship.Parser.ParseFrom);
    #endregion

    #region Methods
    static readonly aelf::Method<global::Points.Contracts.Point.InitializeInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_Initialize = new aelf::Method<global::Points.Contracts.Point.InitializeInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "Initialize",
        __Marshaller_InitializeInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::AElf.Types.Address, global::Google.Protobuf.WellKnownTypes.Empty> __Method_SetAdmin = new aelf::Method<global::AElf.Types.Address, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "SetAdmin",
        __Marshaller_aelf_Address,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Types.Address> __Method_GetAdmin = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Types.Address>(
        aelf::MethodType.View,
        __ServiceName,
        "GetAdmin",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_aelf_Address);

    static readonly aelf::Method<global::Points.Contracts.Point.CreatePointInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_CreatePoint = new aelf::Method<global::Points.Contracts.Point.CreatePointInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "CreatePoint",
        __Marshaller_CreatePointInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Points.Contracts.Point.JoinInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_Join = new aelf::Method<global::Points.Contracts.Point.JoinInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "Join",
        __Marshaller_JoinInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Points.Contracts.Point.SettleInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_Settle = new aelf::Method<global::Points.Contracts.Point.SettleInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "Settle",
        __Marshaller_SettleInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Points.Contracts.Point.RecordRegistrationInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_RecordRegistration = new aelf::Method<global::Points.Contracts.Point.RecordRegistrationInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "RecordRegistration",
        __Marshaller_RecordRegistrationInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Points.Contracts.Point.ApplyToOperatorInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_ApplyToOperator = new aelf::Method<global::Points.Contracts.Point.ApplyToOperatorInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "ApplyToOperator",
        __Marshaller_ApplyToOperatorInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Points.Contracts.Point.PointsSettlementInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_PointsSettlement = new aelf::Method<global::Points.Contracts.Point.PointsSettlementInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "PointsSettlement",
        __Marshaller_PointsSettlementInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty> __Method_SetMaxRecordListCount = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "SetMaxRecordListCount",
        __Marshaller_google_protobuf_Int32Value,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty> __Method_SetMaxRegistrationListCount = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "SetMaxRegistrationListCount",
        __Marshaller_google_protobuf_Int32Value,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty> __Method_SetMaxApplyCount = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "SetMaxApplyCount",
        __Marshaller_google_protobuf_Int32Value,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Points.Contracts.Point.SetReservedDomainListInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_SetReservedDomainList = new aelf::Method<global::Points.Contracts.Point.SetReservedDomainListInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "SetReservedDomainList",
        __Marshaller_SetReservedDomainListInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Points.Contracts.Point.GetReservedDomainListOutput> __Method_GetReservedDomainList = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Points.Contracts.Point.GetReservedDomainListOutput>(
        aelf::MethodType.View,
        __ServiceName,
        "GetReservedDomainList",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_GetReservedDomainListOutput);

    static readonly aelf::Method<global::Points.Contracts.Point.SetServicesEarningRulesInput, global::Google.Protobuf.WellKnownTypes.Empty> __Method_SetServicesEarningRules = new aelf::Method<global::Points.Contracts.Point.SetServicesEarningRulesInput, global::Google.Protobuf.WellKnownTypes.Empty>(
        aelf::MethodType.Action,
        __ServiceName,
        "SetServicesEarningRules",
        __Marshaller_SetServicesEarningRulesInput,
        __Marshaller_google_protobuf_Empty);

    static readonly aelf::Method<global::Points.Contracts.Point.GetServicesEarningRulesInput, global::Points.Contracts.Point.GetServicesEarningRulesOutput> __Method_GetServicesEarningRules = new aelf::Method<global::Points.Contracts.Point.GetServicesEarningRulesInput, global::Points.Contracts.Point.GetServicesEarningRulesOutput>(
        aelf::MethodType.View,
        __ServiceName,
        "GetServicesEarningRules",
        __Marshaller_GetServicesEarningRulesInput,
        __Marshaller_GetServicesEarningRulesOutput);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value> __Method_GetMaxRecordListCount = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value>(
        aelf::MethodType.View,
        __ServiceName,
        "GetMaxRecordListCount",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_Int32Value);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value> __Method_GetMaxApplyCount = new aelf::Method<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value>(
        aelf::MethodType.View,
        __ServiceName,
        "GetMaxApplyCount",
        __Marshaller_google_protobuf_Empty,
        __Marshaller_google_protobuf_Int32Value);

    static readonly aelf::Method<global::Google.Protobuf.WellKnownTypes.StringValue, global::Points.Contracts.Point.DomainOperatorRelationship> __Method_GetDomainApplyInfo = new aelf::Method<global::Google.Protobuf.WellKnownTypes.StringValue, global::Points.Contracts.Point.DomainOperatorRelationship>(
        aelf::MethodType.View,
        __ServiceName,
        "GetDomainApplyInfo",
        __Marshaller_google_protobuf_StringValue,
        __Marshaller_DomainOperatorRelationship);

    #endregion

    #region Descriptors
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Points.Contracts.Point.PointsContractReflection.Descriptor.Services[0]; }
    }

    public static global::System.Collections.Generic.IReadOnlyList<global::Google.Protobuf.Reflection.ServiceDescriptor> Descriptors
    {
      get
      {
        return new global::System.Collections.Generic.List<global::Google.Protobuf.Reflection.ServiceDescriptor>()
        {
          global::AElf.Standards.ACS12.Acs12Reflection.Descriptor.Services[0],
          global::Points.Contracts.Point.PointsContractReflection.Descriptor.Services[0],
        };
      }
    }
    #endregion

    public class PointsContractReferenceState : global::AElf.Sdk.CSharp.State.ContractReferenceState
    {
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Points.Contracts.Point.InitializeInput, global::Google.Protobuf.WellKnownTypes.Empty> Initialize { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::AElf.Types.Address, global::Google.Protobuf.WellKnownTypes.Empty> SetAdmin { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::AElf.Types.Address> GetAdmin { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Points.Contracts.Point.CreatePointInput, global::Google.Protobuf.WellKnownTypes.Empty> CreatePoint { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Points.Contracts.Point.JoinInput, global::Google.Protobuf.WellKnownTypes.Empty> Join { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Points.Contracts.Point.SettleInput, global::Google.Protobuf.WellKnownTypes.Empty> Settle { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Points.Contracts.Point.RecordRegistrationInput, global::Google.Protobuf.WellKnownTypes.Empty> RecordRegistration { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Points.Contracts.Point.ApplyToOperatorInput, global::Google.Protobuf.WellKnownTypes.Empty> ApplyToOperator { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Points.Contracts.Point.PointsSettlementInput, global::Google.Protobuf.WellKnownTypes.Empty> PointsSettlement { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty> SetMaxRecordListCount { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty> SetMaxRegistrationListCount { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Int32Value, global::Google.Protobuf.WellKnownTypes.Empty> SetMaxApplyCount { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Points.Contracts.Point.SetReservedDomainListInput, global::Google.Protobuf.WellKnownTypes.Empty> SetReservedDomainList { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Points.Contracts.Point.GetReservedDomainListOutput> GetReservedDomainList { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Points.Contracts.Point.SetServicesEarningRulesInput, global::Google.Protobuf.WellKnownTypes.Empty> SetServicesEarningRules { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Points.Contracts.Point.GetServicesEarningRulesInput, global::Points.Contracts.Point.GetServicesEarningRulesOutput> GetServicesEarningRules { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value> GetMaxRecordListCount { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.Empty, global::Google.Protobuf.WellKnownTypes.Int32Value> GetMaxApplyCount { get; set; }
      internal global::AElf.Sdk.CSharp.State.MethodReference<global::Google.Protobuf.WellKnownTypes.StringValue, global::Points.Contracts.Point.DomainOperatorRelationship> GetDomainApplyInfo { get; set; }
    }
  }
}
#endregion

