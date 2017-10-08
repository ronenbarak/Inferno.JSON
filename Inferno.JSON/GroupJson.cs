using System;
using System.IO;
using Inferno.JSONSerilizer;
using Inferno.JSONSerilizer.OtherSerilizers.Class;
using Inferno.JSONSerilizer.PrimitiveSerilizers;

namespace Inferno
{
  class GroupJson : IJSON
  {
    private JSONConfiguration m_jsonConfiguration;
    private readonly TypeSerilizerRepository m_typeSerilizerRepository =new TypeSerilizerRepository();

    public GroupJson(JSONConfiguration configuration)
    {
      m_jsonConfiguration = configuration;
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<bool>(new EntityBoolSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<bool?>(new EntityBoolNullSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<byte>(new EntityByteSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<byte?>(new EntityByteNullSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<char>(new EntityCharSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<char?>(new EntityCharNullSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<decimal>(new EntityDecimalSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<decimal?>(new EntityDecimalNullSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<double>(new EntityDoubleSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<double?>(new EntityDoubleNullSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<float>(new EntityFloatSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<float?>(new EntityFloatNullSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<Guid>(new EntityGuidSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<Guid?>(new EntityGuidNullSerilier(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<int>(new EntityIntSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<int?>(new EntityIntNullSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<long>(new EntityLongSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<long?>(new EntityLongNullSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<DateTime>(new EntityDateTimeSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<DateTime?>(new EntityDateTimeNullSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<sbyte>(new EntitySByteSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<sbyte?>(new EntitySByteNullSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<short>(new EntityShortSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<short?>(new EntityShortNullSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<string>(new EntityStringSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<TimeSpan>(new EntityTimeSpanSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<TimeSpan?>(new EntityTimeSpanNullSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<uint>(new EntityUIntSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<uint?>(new EntityUIntNullSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<ulong>(new EntityULongSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<ulong?>(new EntityULongNullSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<ushort>(new EntityUShortSerilizer(m_jsonConfiguration)));
      m_typeSerilizerRepository.AddSerilizer(new ClassSerilizerFactory<ushort?>(new EntityUShortNullSerilizer(m_jsonConfiguration)));
    }

    public IJSONSerilizer<T> CreateTypeSerilizer<T>(Action<ISerilizerDefinition<T>> action)
    {
      var sessionPack = new SessionSerilizerPack(m_typeSerilizerRepository);
      SerilizerDefinition<T> serilizerDefinition = new SerilizerDefinition<T>(m_jsonConfiguration, sessionPack, this);
      action.Invoke(serilizerDefinition);
      var serilizer = serilizerDefinition.CreateSerilizerNoCache();
      sessionPack.Flush();
      return serilizer;
    }

    public IJSONSerilizer<T> CreateTypeSerilizer<T>()
    {
      var sessionPack = new SessionSerilizerPack(m_typeSerilizerRepository);
      SerilizerDefinition<T> serilizerDefinition = new SerilizerDefinition<T>(m_jsonConfiguration, sessionPack, this);
      serilizerDefinition.AutoDefine();
      var serilizer = serilizerDefinition.CreateSerilizer();
      sessionPack.Flush();
      return serilizer;
    }

    /*internal IJSONSerilizer<T> CreateTypeSerilizer<T>(Action<ISerilizerDefinition<T>> action, SessionSerilizerPack sessionSerilizerPack)
    {
      SerilizerDefinition<T> serilizerDefinition = new SerilizerDefinition<T>(m_jsonConfiguration,sessionSerilizerPack,this);
      action.Invoke(serilizerDefinition);
      var serilizer = serilizerDefinition.CreateSerilizer();
      return serilizer;
    }*/

    public string Serilize<T>(T obj)
    {
      StringWriter stringWriter = new StringWriter();
      IJSONSerilizer<T> serilizer;
      if (!m_typeSerilizerRepository.TryGetSerilizer<T>(out serilizer))
      {
        serilizer = CreateTypeSerilizer<T>();
      }
      serilizer.Serilize(obj,stringWriter);
      return stringWriter.ToString();
    }
  }
}