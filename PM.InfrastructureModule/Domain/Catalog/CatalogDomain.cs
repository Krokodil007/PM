using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PM.InfrastructureModule.Common.Data;
using PM.InfrastructureModule.Domain.Services.Catalog;
using PM.InfrastructureModule.Dto.Catalog;
using PM.InfrastructureModule.Entity.Catalog;
using PM.InfrastructureModule.Entity.Catalog.Json;
using PM.InfrastructureModule.Repository.StaticQuery.Catalog;

namespace PM.InfrastructureModule.Domain.Catalog
{
    /// <summary>
    /// Справочники
    /// </summary>
    public class CatalogDomain : ICatalogDomain
    {
        private List<PaperListTypeDto> _paperListTypeList;
        private List<ManufacturerDto> _manufacturerList;
        private List<MeasureUnitDto> _measureUnitList;

        /// <summary>
        /// Catalog Get
        /// </summary>
        public async Task<IEnumerable<object>> CatalogGet(string objectTypeName, string objectGuid,
            string serviceGroupGuid, string filterString)
        {
            var result = await UniversalCatalogInfo.CatalogObjectGet(objectTypeName, objectGuid, serviceGroupGuid);
            var mappedResult = await MapCatalogGet(objectTypeName, result, filterString, serviceGroupGuid);

            return mappedResult;
        }

        /// <summary>
        /// Catalog Upd
        /// </summary>
        public async Task<object> CatalogUpd(string objectTypeName, string item, string userGuid, string serviceGroupGuid)
        {
            IEnumerable<UniversalCatalogEntity> result;

            switch (objectTypeName)
            {
                case "order_type":
                    var ot = JsonDataExtensions.JsonToEntityData<OrderTypeDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(ot.order_type_guid,
                        ot.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new OrderTypeEntity
                            {order_type_name = ot.order_type_name}));
                    break;
                case "order_status":
                    var os = JsonDataExtensions.JsonToEntityData<OrderStatusDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(os.order_status_guid,
                        os.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new OrderStatusEntity
                            {order_status_name = os.order_status_name}));
                    break;
                case "nomenclature_type":
                    var nt = JsonDataExtensions.JsonToEntityData<NomenclatureTypeDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(nt.nomenclature_type_guid,
                        nt.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new NomenclatureTypeEntity
                        {
                            nomenclature_type_name = nt.nomenclature_type_name,
                            nomenclature_type_form_number = nt.nomenclature_type_form_number,
                            order_type_guid = nt.order_type_guid
                        }));
                    break;
                case "nomenclature_format":
                    var nlf = JsonDataExtensions.JsonToEntityData<NomenclatureFormatDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(nlf.nomenclature_format_guid,
                        nlf.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new NomenclatureFormatEntity
                        {
                            nomenclature_format_name = nlf.nomenclature_format_name, width = nlf.width,
                            height = nlf.height, nomenclature_format_form_number = nlf.nomenclature_format_form_number
                        }));
                    break;
                case "contact_type":
                    var ct = JsonDataExtensions.JsonToEntityData<ContactTypeDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(ct.contact_type_guid,
                        ct.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(
                            new OrderTypeEntity {order_type_name = ct.contact_type_name}));
                    break;
                case "ownership_type":
                    var owt = JsonDataExtensions.JsonToEntityData<OwnershipTypeDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(owt.ownership_type_guid,
                        owt.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new OwnershipTypeEntity
                        {
                            ownership_type_abbreviation = owt.ownership_type_abbreviation,
                            ownership_type_name = owt.ownership_type_name
                        }));
                    break;
                case "measure_unit":
                    var mu = JsonDataExtensions.JsonToEntityData<MeasureUnitDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(mu.measure_unit_guid,
                        mu.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new MeasureUnitEntity
                        {
                            measure_unit_abbreviation = mu.measure_unit_abbreviation,
                            measure_unit_name = mu.measure_unit_name
                        }));
                    break;
                case "manufacturer":
                    var mf = JsonDataExtensions.JsonToEntityData<ManufacturerDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(mf.manufacturer_guid,
                        mf.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new ManufacturerEntity
                            {manufacturer_name = mf.manufacturer_name}));
                    break;
                case "paper_list_type":
                    var plt = JsonDataExtensions.JsonToEntityData<PaperListTypeDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(plt.paper_list_type_guid,
                        plt.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new PaperListTypeEntity
                            {paper_list_type_name = plt.paper_list_type_name}));
                    break;
                case "paper_list":
                    var pl = JsonDataExtensions.JsonToEntityData<PaperListDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(pl.paper_list_guid,
                        pl.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new PaperListEntity
                        {
                            material_type_guid = pl.material_type_guid, paper_list_name = pl.paper_list_name,
                            vendor_code = pl.vendor_code, characteristic = pl.characteristic,
                            paper_list_type_guid = pl.paper_list_type_guid, height = pl.height, width = pl.width,
                            thickness = pl.thickness, density = pl.density, quantity = pl.quantity, price = pl.price,
                            measure_unit_guid = pl.measure_unit_guid, manufacturer_guid = pl.manufacturer_guid
                        }));
                    break;
                case "equipment_type":
                    var et = JsonDataExtensions.JsonToEntityData<EquipmentTypeDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(et.equipment_type_guid,
                        et.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new EquipmentTypeEntity
                            {equipment_type_name = et.equipment_type_name}));
                    break;
                case "cover_type":
                    var cvt = JsonDataExtensions.JsonToEntityData<CoverTypeDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(cvt.cover_type_guid,
                        cvt.object_type_guid, serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new CoverTypeEntity
                            {cover_type_name = cvt.cover_type_name}));
                    break;
                case "material_type":
                    var mt = JsonDataExtensions.JsonToEntityData<MaterialTypeDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(mt.material_type_guid, mt.object_type_guid,
                        serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new MaterialTypeEntity
                            {material_type_name = mt.material_type_name}));
                    break;
                case "operation":
                    var op = JsonDataExtensions.JsonToEntityData<OperationDto>(item);
                    result = await UniversalCatalogInfo.CatalogObjectUpd(op.operation_guid, op.object_type_guid,
                        serviceGroupGuid, userGuid,
                        JsonDataExtensions.EntityToJsonData(new OperationEntity
                        {
                            operation_name = op.operation_name, measure_unit_guid = op.measure_unit_guid,
                            operation_price = op.operation_price, operation_complexity = op.operation_complexity
                        }));
                    break;
                default:
                    result = null;
                    break;
            }

            var resultx = await MapCatalogGet(objectTypeName, result, null, serviceGroupGuid);
            return resultx.FirstOrDefault();
        }

        private async Task<IEnumerable<object>> MapCatalogGet(string objectTypeName,
            IEnumerable<UniversalCatalogEntity> items,
            string filterString, string serviceGroupGuid)
        {
            var result = new List<object>();
            foreach (var r in items)
            {
                switch (objectTypeName)
                {
                    case "order_type":
                        var ot = JsonDataExtensions.JsonToEntityData<OrderTypeDto>(r.object_info);
                        ot.order_type_guid = r.object_guid;
                        ot.object_type_guid = r.object_type_guid;
                        result.Add(ot);
                        break;
                    case "order_status":
                        var os = JsonDataExtensions.JsonToEntityData<OrderStatusDto>(r.object_info);
                        os.order_status_guid = r.object_guid;
                        os.object_type_guid = r.object_type_guid;
                        result.Add(os);
                        break;
                    case "nomenclature_type":
                        var nt = JsonDataExtensions.JsonToEntityData<NomenclatureTypeDto>(r.object_info);
                        nt.nomenclature_type_guid = r.object_guid;
                        nt.object_type_guid = r.object_type_guid;
                        if (!string.IsNullOrEmpty(filterString))
                        {
                            if (nt.order_type_guid.Contains(filterString)) result.Add(nt);
                        }
                        else
                        {
                            result.Add(nt);
                        }

                        break;
                    case "nomenclature_format":
                        var nlf = JsonDataExtensions.JsonToEntityData<NomenclatureFormatDto>(r.object_info);
                        nlf.nomenclature_format_guid = r.object_guid;
                        nlf.object_type_guid = r.object_type_guid;
                        result.Add(nlf);
                        break;
                    case "contact_type":
                        var ct = JsonDataExtensions.JsonToEntityData<ContactTypeDto>(r.object_info);
                        ct.contact_type_guid = r.object_guid;
                        ct.object_type_guid = r.object_type_guid;
                        result.Add(ct);
                        break;
                    case "ownership_type":
                        var owt = JsonDataExtensions.JsonToEntityData<OwnershipTypeDto>(r.object_info);
                        owt.ownership_type_guid = r.object_guid;
                        owt.object_type_guid = r.object_type_guid;
                        result.Add(owt);
                        break;
                    case "measure_unit":
                        var mu = JsonDataExtensions.JsonToEntityData<MeasureUnitDto>(r.object_info);
                        mu.measure_unit_guid = r.object_guid;
                        mu.object_type_guid = r.object_type_guid;
                        result.Add(mu);
                        break;
                    case "manufacturer":
                        var mf = JsonDataExtensions.JsonToEntityData<ManufacturerDto>(r.object_info);
                        mf.manufacturer_guid = r.object_guid;
                        mf.object_type_guid = r.object_type_guid;
                        result.Add(mf);
                        break;
                    case "paper_list_type":
                        var plt = JsonDataExtensions.JsonToEntityData<PaperListTypeDto>(r.object_info);
                        plt.paper_list_type_guid = r.object_guid;
                        plt.object_type_guid = r.object_type_guid;
                        result.Add(plt);
                        break;
                    case "paper_list":
                        var pl = JsonDataExtensions.JsonToEntityData<PaperListDto>(r.object_info);
                        pl.paper_list_guid = r.object_guid;
                        pl.object_type_guid = r.object_type_guid;
                        var paperListType = await PaperListTypeGet(r.service_group_guid);
                        pl.paper_list_type_name = paperListType.Where(c => c.paper_list_type_guid == pl.paper_list_type_guid)
                            .Select(c => c.paper_list_type_name).FirstOrDefault();
                        var manufacturer = await ManufacturerGet(r.service_group_guid);
                        pl.manufacturer_name = manufacturer.Where(c => c.manufacturer_guid == pl.manufacturer_guid)
                            .Select(c => c.manufacturer_name).FirstOrDefault();
                        var measureUnit = await MeasureUnitGet(r.service_group_guid);
                        pl.measure_unit_name = measureUnit.Where(c => c.measure_unit_guid == pl.measure_unit_guid)
                            .Select(c => c.measure_unit_name).FirstOrDefault();
                        result.Add(pl);
                        break;
                    case "equipment_type":
                        var et = JsonDataExtensions.JsonToEntityData<EquipmentTypeDto>(r.object_info);
                        et.equipment_type_guid = r.object_guid;
                        et.object_type_guid = r.object_type_guid;
                        result.Add(et);
                        break;
                    case "cover_type":
                        var cvt = JsonDataExtensions.JsonToEntityData<CoverTypeDto>(r.object_info);
                        cvt.cover_type_guid = r.object_guid;
                        cvt.object_type_guid = r.object_type_guid;
                        result.Add(cvt);
                        break;
                    case "material_type":
                        var mt = JsonDataExtensions.JsonToEntityData<MaterialTypeDto>(r.object_info);
                        mt.material_type_guid = r.object_guid;
                        mt.object_type_guid = r.object_type_guid;
                        result.Add(mt);
                        break;
                    case "operation":
                        var op = JsonDataExtensions.JsonToEntityData<OperationDto>(r.object_info);
                        op.operation_guid = r.object_guid;
                        op.object_type_guid = r.object_type_guid;
                        measureUnit = await MeasureUnitGet(r.service_group_guid);
                        op.measure_unit_name = measureUnit.Where(c => c.measure_unit_guid == op.measure_unit_guid)
                            .Select(c => c.measure_unit_name).FirstOrDefault();
                        result.Add(op);
                        break;
                    default:
                        result = null;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Получение списка типов листовой бумаги
        /// </summary>
        private async Task<IEnumerable<PaperListTypeDto>> PaperListTypeGet(string serviceGroupGuid)
        {
            if (_paperListTypeList == null)
            {
                var result = await CatalogGet("paper_list_type", null, serviceGroupGuid, null);
                var list = result.Select(v => v as PaperListTypeDto).ToList();

                _paperListTypeList = list;
                return list;
            }

            return _paperListTypeList;
        }

        /// <summary>
        /// Получение списка производителей
        /// </summary>
        private async Task<IEnumerable<ManufacturerDto>> ManufacturerGet(string serviceGroupGuid)
        {
            if (_manufacturerList == null)
            {
                var result = await CatalogGet("manufacturer", null, serviceGroupGuid, null);
                var list = result.Select(v => v as ManufacturerDto).ToList();

                _manufacturerList = list;
                return list;
            }

            return _manufacturerList;
        }

        /// <summary>
        /// Получение списка единиц измерения
        /// </summary>
        private async Task<IEnumerable<MeasureUnitDto>> MeasureUnitGet(string serviceGroupGuid)
        {
            if (_measureUnitList == null)
            {
                var result = await CatalogGet("measure_unit", null, serviceGroupGuid, null);
                var list = result.Select(v => v as MeasureUnitDto).ToList();

                _measureUnitList = list;
                return list;
            }

            return _measureUnitList;
        }
    }
}