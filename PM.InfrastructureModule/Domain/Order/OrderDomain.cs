using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PM.InfrastructureModule.Common.App;
using PM.InfrastructureModule.Common.Data;
using PM.InfrastructureModule.Domain.Catalog;
using PM.InfrastructureModule.Domain.Contact;
using PM.InfrastructureModule.Domain.Equipment;
using PM.InfrastructureModule.Domain.Services.Catalog;
using PM.InfrastructureModule.Domain.Services.Contact;
using PM.InfrastructureModule.Domain.Services.Equipment;
using PM.InfrastructureModule.Domain.Services.Order;
using PM.InfrastructureModule.Dto.Catalog;
using PM.InfrastructureModule.Dto.Order.In;
using PM.InfrastructureModule.Dto.Order.InOut;
using PM.InfrastructureModule.Dto.Order.Out;
using PM.InfrastructureModule.Entity.Order;
using PM.InfrastructureModule.Repository.StaticQuery.Equipment;
using PM.InfrastructureModule.Repository.StaticQuery.Order;
using Unity;
using NomenclatureMaterialOutDto = PM.InfrastructureModule.Dto.Order.Out.NomenclatureMaterialOutDto;
using NomenclatureOperationOutDto = PM.InfrastructureModule.Dto.Order.Out.NomenclatureOperationOutDto;

namespace PM.InfrastructureModule.Domain.Order
{
    /// <summary>
    /// Заказы
    /// </summary>
    public class OrderDomain : IOrderDomain
    {

        [UsedImplicitly] private readonly IEquipmentDomain _equipmentDomain;
        [UsedImplicitly] private readonly ICatalogDomain _catalogDomain;
        [UsedImplicitly] private readonly IContactDomain _contactDomain;

        public OrderDomain()
        {
            _equipmentDomain = Bootstraper.Init().Resolve<EquipmentDomain>();
            _catalogDomain = Bootstraper.Init().Resolve<CatalogDomain>();
            _contactDomain = Bootstraper.Init().Resolve<ContactDomain>();
        }

        /// <summary>
        /// Order Get
        /// </summary>
        public async Task<IEnumerable<OrderDto>> OrderGet(string orderGuid, string serviceGroupGuid)
        {
            var orders = await OrderInfo.OrderGet(orderGuid, serviceGroupGuid);
            var orderEntities = orders.ToList();
            var result = orderEntities.Select(c => JsonDataExtensions.JsonToEntityData<OrderDto>(c.order_info))
                .ToList();
            foreach (var v in result)
            {
                v.user_fullname = orderEntities.Where(c => c.order_guid == v.order_guid).Select(c => c.user_fullname)
                    .FirstOrDefault();
                v.contact_name = orderEntities.Where(c => c.order_guid == v.order_guid).Select(c => c.contact_name)
                    .FirstOrDefault();
                v.order_type_name = orderEntities.Where(c => c.order_guid == v.order_guid)
                    .Select(c => c.order_type_name)
                    .FirstOrDefault();
                v.order_status_name = orderEntities.Where(c => c.order_guid == v.order_guid)
                    .Select(c => c.order_status_name)
                    .FirstOrDefault();
                v.dtcreate = orderEntities.Where(c => c.order_guid == v.order_guid).Select(c => c.dtcreate)
                    .FirstOrDefault();
            }

            if (orderGuid != null)
            {
                foreach (var v in result)
                {
                    var account = await _contactDomain.ContactBankAccountGet(null, v.contact_guid, serviceGroupGuid);
                    v.order_vat_value = account.Select(a => a.vat).FirstOrDefault();
                }
            }

            return result;
        }

        /// <summary>
        /// Order Upd
        /// </summary>
        public async Task<OrderDto> OrderUpd(OrderDto dtoItem, string userGuid, string serviceGroupGuid)
        {
            var item = new OrderEntity
            {
                order_guid = dtoItem.order_guid,
                contact_guid = dtoItem.contact_guid,
                order_type_guid = dtoItem.order_type_guid,
                order_status_guid = dtoItem.order_status_guid,
                order_info = JsonDataExtensions.EntityToJsonData(dtoItem)
            };

            var order = await OrderInfo.OrderUpd(item, userGuid, serviceGroupGuid);
            var result = JsonDataExtensions.JsonToEntityData<OrderDto>(order.order_info);



            return result;
        }

        /// <summary>
        /// Nomenclature Get
        /// </summary>
        public async Task<IEnumerable<NomenclatureOutDto>> NomenclatureGet(string nomenclatureGuid, string orderGuid,
            string serviceGroupGuid)
        {
            var nomenclature = await OrderInfo.NomenclatureGet(nomenclatureGuid, orderGuid, serviceGroupGuid);
            var result = nomenclature
                .Select(c => JsonDataExtensions.JsonToEntityData<NomenclatureOutDto>(c.nomenclature_info)).ToList();

            foreach (var r in result)
            {
                var nomenclatureOperation =
                    await OrderInfo.NomenclatureOperationGet(null, r.nomenclature_guid, serviceGroupGuid);
                var nomenclatureOperationList = nomenclatureOperation.Select(c => new NomenclatureOperationOutDto
                {
                    nomenclature_guid = c.nomenclature_guid,
                    operation_guid = c.operation_guid,
                    operation_price = c.operation_price,
                    operation_count = c.operation_count,
                    operation_price_total = c.operation_price_total,
                    operation_price_total_src = c.operation_price_total_src,
                    operation_attribute =
                        JsonDataExtensions.JsonToEntityData<NomenclatureOperationAttribute>(c.operation_attribute)
                });

                var operationDto = await NomenclatureOperationDtoDataAdd(nomenclatureOperationList, serviceGroupGuid);
                r.nomenclature_operation = operationDto.ToList();

                var nomenclatureMaterial =
                    await OrderInfo.NomenclatureMaterialGet(null, nomenclatureGuid, serviceGroupGuid);
                var nomenclatureMaterialList = nomenclatureMaterial.Select(c => new NomenclatureMaterialOutDto
                {
                    nomenclature_guid = c.nomenclature_guid,
                    material_guid = c.material_guid,
                    material_price = c.material_price,
                    material_count = c.material_count,
                    material_price_total = c.material_price_total,
                    material_price_total_src = c.material_price_total_src,
                    material_attribute =
                        JsonDataExtensions.JsonToEntityData<NomenclatureMaterialAttribute>(c.material_attribute)
                });
                var materialDto = await NomenclatureMaterialDtoDataAdd(nomenclatureMaterialList, serviceGroupGuid);
                r.nomenclature_material = materialDto.ToList();
            }

            return result;
        }

        /// <summary>
        /// Заполняем поля списка работ
        /// </summary>
        private async Task<IEnumerable<NomenclatureOperationOutDto>> NomenclatureOperationDtoDataAdd(
            IEnumerable<NomenclatureOperationOutDto> operationIn, string serviceGroupGuid)
        {
            var operation = await _catalogDomain.CatalogGet("operation", null, serviceGroupGuid, null);
            var operationList = operation.Select(v => v as OperationDto).ToList();
            var measureUnit = await _catalogDomain.CatalogGet("measure_unit", null, serviceGroupGuid, null);
            var measureUnitList = measureUnit.Select(v => v as MeasureUnitDto).ToList();
            var operationInList = operationIn.ToList();
            foreach (var r in operationInList.ToList())
            {
                r.operation_name = operationList.Where(o => o.operation_guid == r.operation_guid)
                    .Select(o => o.operation_name).FirstOrDefault();
                r.measure_guid = operationList.Where(o => o.operation_guid == r.operation_guid)
                    .Select(o => o.measure_unit_guid).FirstOrDefault();
                r.measure_name = measureUnitList
                    .Where(m => m.measure_unit_guid == operationList
                                    .Where(o => o.operation_guid == r.operation_guid).Select(o => o.measure_unit_guid)
                                    .FirstOrDefault()).Select(m => m.measure_unit_name).FirstOrDefault();
                r.operation_price = operationList.Where(o => o.operation_guid == r.operation_guid)
                    .Select(o => o.operation_price).FirstOrDefault();
            }

            return operationInList;
        }

        /// <summary>
        /// Заполняем поля списка материалов
        /// </summary>
        private async Task<IEnumerable<NomenclatureMaterialOutDto>> NomenclatureMaterialDtoDataAdd(
            IEnumerable<NomenclatureMaterialOutDto> materialIn, string serviceGroupGuid)
        {
            //TODO: доделать обработку материалов
            var material = await _catalogDomain.CatalogGet("paper_list", null, serviceGroupGuid, null);
            var materialList = material.Select(v => v as PaperListDto).ToList();
            var measureUnit = await _catalogDomain.CatalogGet("measure_unit", null, serviceGroupGuid, null);
            var measureUnitList = measureUnit.Select(v => v as MeasureUnitDto).ToList();
            var materialInList = materialIn.ToList();
            foreach (var r in materialInList.ToList())
            {
                r.material_type_guid = materialList.Where(o => o.paper_list_guid == r.material_guid)
                    .Select(o => o.material_type_guid).FirstOrDefault();
                r.material_name = materialList.Where(o => o.paper_list_guid == r.material_guid)
                    .Select(o => o.paper_list_name).FirstOrDefault();
                r.measure_guid = materialList.Where(o => o.paper_list_guid == r.material_guid)
                    .Select(o => o.measure_unit_guid).FirstOrDefault();
                r.measure_name = measureUnitList
                    .Where(m => m.measure_unit_guid == materialList
                                    .Where(o => o.paper_list_guid == r.material_guid).Select(o => o.measure_unit_guid)
                                    .FirstOrDefault()).Select(m => m.measure_unit_name).FirstOrDefault();
                r.material_price = materialList.Where(o => o.paper_list_guid == r.material_guid)
                    .Select(o => o.price).FirstOrDefault();
            }

            return materialInList;
        }

        /// <summary>
        /// Nomenclature Upd
        /// </summary>
        public async Task<List<NomenclatureOutDto>> NomenclatureUpd(List<NomenclatureOutDto> dtoItemList,
            string userGuid,
            string serviceGroupGuid)
        {
            foreach (var dtoItem in dtoItemList)
            {
                var item = new NomenclatureEntity
                {
                    nomenclature_guid = dtoItem.nomenclature_guid,
                    nomenclature_type_guid = dtoItem.nomenclature_type_guid,
                    order_guid = dtoItem.order_guid,
                    nomenclature_info = JsonDataExtensions.EntityToJsonData(dtoItem)
                };
                await OrderInfo.NomenclatureUpd(item, userGuid, serviceGroupGuid);

                await OrderInfo.NomenclatureMaterialByNomenclatureDel(dtoItem.nomenclature_guid, serviceGroupGuid);
                await OrderInfo.NomenclatureOperationByNomenclatureDel(dtoItem.nomenclature_guid, serviceGroupGuid);

                foreach (var no in dtoItem.nomenclature_operation)
                {
                    var noItem = new NomenclatureOperationEntity
                    {
                        service_group_guid = serviceGroupGuid,
                        user_guid = userGuid,
                        nomenclature_guid = dtoItem.nomenclature_guid,
                        operation_guid = no.operation_guid,
                        operation_count = no.operation_count,
                        operation_price = no.operation_price,
                        operation_price_total = no.operation_price_total,
                        operation_price_total_src = no.operation_price_total_src,
                        operation_attribute = JsonDataExtensions.EntityToJsonData(no.operation_attribute)
                    };
                    await OrderInfo.NomenclatureOperationUpd(noItem, userGuid, serviceGroupGuid);
                }

                foreach (var no in dtoItem.nomenclature_material)
                {
                    var noItem = new NomenclatureMaterialEntity
                    {
                        service_group_guid = serviceGroupGuid,
                        user_guid = userGuid,
                        nomenclature_guid = dtoItem.nomenclature_guid,
                        material_guid = no.material_guid,
                        material_count = no.material_count,
                        material_price = no.material_price,
                        material_price_total = no.material_price_total,
                        material_price_total_src = no.material_price_total_src,
                        material_attribute = JsonDataExtensions.EntityToJsonData(no.material_attribute)
                    };
                    await OrderInfo.NomenclatureMaterialUpd(noItem, userGuid, serviceGroupGuid);
                }
            }

            return dtoItemList;
        }

        /// <summary>
        /// Nomenclature Del
        /// </summary>
        public async Task NomenclatureDel(List<string> nomenclatureGuidList, string serviceGroupGuid)
        {
            foreach (var noItem in nomenclatureGuidList)
            {
                await OrderInfo.NomenclatureDel(noItem, serviceGroupGuid);
            }
        }

        /// <summary>
        ///  Расчет доли резки до печати 
        /// </summary>
        private int CalculateProportionResult(decimal a, decimal b, decimal c, decimal d)
        {
            if (a > b)
            {
                var a0 = a;
                var b0 = b;
                b = a0;
                a = b0;
            }

            var k = 0;

            for (int i = 1; i <= 32; i++)
            {
                MaxMultiplier(i, out var m1, out var m2);
                if ((a / m1) <= (c + 5) || (a / m1) <= (c + 5))
                    if ((b / m2) <= (d + 5) || (b / m2) <= (d + 5))
                    {
                        k = i;
                        break;
                    }

                if ((a / m1) <= (d + 5) || (a / m1) <= (d + 5))
                    if ((b / m2) <= (c + 5) || (b / m2) <= (c + 5))
                    {
                        k = i;
                        break;
                    }
            }

            if (k > 32)
                k = 1;

            return k;
        }

        /// <summary>
        /// Расчет рабочего формата
        /// </summary>
        private void CalculateWorkFormat(int dol, decimal a, decimal b, decimal c, decimal d, out decimal f1,
            out decimal f2)
        {

            decimal maxWork1 = 0;
            decimal maxWork2 = 0;

            if (b > 0)
            {
                CuttingFormat(dol, a, b, out a, out b);
            }

            if (c > 0 && d > 0)
            {
                maxWork1 = c;
                maxWork2 = d;
                if (maxWork1 > maxWork2)
                {
                    var t = maxWork1;
                    maxWork1 = maxWork2;
                    maxWork2 = t;
                }
            }
            else
            {
                maxWork1 = a;
                maxWork2 = b;
            }

            if (a > 0)
            {
                f1 = maxWork1 < a ? maxWork1 : a;
            }
            else
            {
                f1 = maxWork1;
            }

            if (b > 0)
            {
                f2 = maxWork2 < b ? maxWork2 : b;
            }
            else
            {
                f2 = maxWork2;
            }
        }

        /// <summary>
        /// Максимальный множитель
        /// </summary>
        private static void MaxMultiplier(int num, out int m1, out int m2)
        {
            m1 = 0;
            m2 = 0;

            for (var i = 1; i <= num; i++)
            {
                for (var j = i; j <= num; j++)
                {
                    var num1 = i * j;
                    if (num1 == num)
                    {
                        if ((i > m1) || (j > m2))
                        {
                            m1 = i;
                            m2 = j;
                        }
                    }
                }
            }

            if (m1 > m2)
            {
                var m = m1;
                m1 = m2;
                m2 = m;
            }
        }

        /// <summary>
        /// Формат с резкой
        /// </summary>
        private static void CuttingFormat(int dol, decimal f01, decimal f02, out decimal f1, out decimal f2)
        {
            decimal retX;
            decimal retY;

            if (dol <= 0)
                dol = 1;

            MaxMultiplier(dol, out var aMax, out var bMax);

            if (aMax < bMax)
            {
                var tmp = aMax;
                aMax = bMax;
                bMax = tmp;
            }

            if (f01 > f02)
            {
                retX = f01 / aMax;
                retY = f02 / bMax;
            }
            else
            {
                retX = f02 / aMax;
                retY = f01 / bMax;
            }


            if (retX > retY)
            {
                f1 = retY;
                f2 = retX;
            }
            else
            {
                f1 = retX;
                f2 = retY;
            }
        }

        /// <summary>
        /// Расчет тетрадей
        /// </summary>
        private int CalcNotebook(int numberOfBands, decimal resultCountProductOneList)
        {
            var aT = 0;
            var miniAt = 0;
            var microAt = 0;
            if (resultCountProductOneList > 16)
                resultCountProductOneList = 16;

            var fT = Math.Floor(numberOfBands / resultCountProductOneList / 2);
            var nnn = numberOfBands / resultCountProductOneList / 2;
            var tempAt = nnn.Fraction();

            while (tempAt > 0)
            {
                if (tempAt >= 0.5)
                {
                    aT = aT + 1;
                    tempAt = (int) (tempAt - 0.5);
                }

                if (tempAt >= 0.25)
                {
                    miniAt = miniAt + 1;
                    tempAt = (int) (tempAt - 0.25);
                }

                if (tempAt >= 0.125)
                {
                    microAt = microAt + 1;
                    tempAt = (int) (tempAt - 0.125);
                }
                else
                    break;
            }

            return (int) Math.Ceiling(fT + aT + miniAt + microAt);
        }

        /// <summary>
        /// K
        /// </summary>
        private static double GetK(int x, double k)
        {
            if (x >= 20 && x <= 24)
                k = 1.2;
            if (x >= 25 && x <= 29)
                k = 1.17;
            if (x >= 30 && x <= 34)
                k = 1.15;
            if (x >= 35 && x <= 39)
                k = 1.12;
            if (x >= 40)
                k = 1.1;
            return k;
        }

        /// <summary>
        /// Расчет номенклатуры листовой
        /// </summary>
        public async Task<NomenclatureOutDto> NomenclatureCalculateForm1(NomenclatureForm1InDto nomenclatureItem,
            string serviceGroupGuid)
        {
            var nomenclatureDto = new NomenclatureOutDto
            {
                nomenclature_guid = nomenclatureItem.nomenclature_guid,
                nomenclature_type_guid = nomenclatureItem.nomenclature_type_guid,
                order_guid = nomenclatureItem.order_guid,
                order_discount = nomenclatureItem.order_discount ?? 0,
                order_markup = nomenclatureItem.order_markup ?? 0,
                nomenclature_name = nomenclatureItem.nomenclature_name,
                nomenclature_count = nomenclatureItem.nomenclature_count ?? 0,
                format_guid = nomenclatureItem.format_guid,
                format_width = nomenclatureItem.format_width ?? 0,
                format_height = nomenclatureItem.format_height ?? 0,
                colorfulness1 = nomenclatureItem.colorfulness1 ?? 0,
                colorfulness2 = nomenclatureItem.colorfulness2 ?? 0,
                equipment_guid = nomenclatureItem.equipment_guid,
                complexity = nomenclatureItem.complexity ?? 0,
                priladki_count = nomenclatureItem.priladki_count ?? 0,
                contact_guid = nomenclatureItem.contact_guid,
                nomenclature_message = new List<string>()
            };

            var operationList = new List<NomenclatureOperationOutDto>();
            foreach (var no in nomenclatureItem.nomenclature_operation)
            {
                var o = new NomenclatureOperationOutDto
                {
                    operation_guid = no.operation_guid,
                    operation_count = no.operation_count,
                    operation_attribute = no.operation_attribute
                };
                operationList.Add(o);
            }

            var operationDto = await NomenclatureOperationDtoDataAdd(operationList, serviceGroupGuid);
            nomenclatureDto.nomenclature_operation = operationDto.ToList();

            var materialList = new List<NomenclatureMaterialOutDto>();
            foreach (var no in nomenclatureItem.nomenclature_material)
            {
                var o = new NomenclatureMaterialOutDto
                {
                    material_guid = no.material_guid,
                    material_count = no.material_count,
                    material_attribute = no.material_attribute
                };
                materialList.Add(o);
            }

            var materialDto = await NomenclatureMaterialDtoDataAdd(materialList, serviceGroupGuid);
            nomenclatureDto.nomenclature_material = materialDto.ToList();

            var result = await NomenclatureCalculateList(nomenclatureDto, serviceGroupGuid);
            if (result.nomenclature_message.Count > 0) return result;

            result = await NomenclatureOperationCalculate(result, serviceGroupGuid);
            if (result.nomenclature_message.Count > 0) return result;

            result = await NomenclatureMaterialCalculateForm1(result, serviceGroupGuid);
            if (result.nomenclature_message.Count > 0) return result;

            result = NomenclatureDiscountMarkupCalculate(result);
            if (result.nomenclature_message.Count > 0) return result;

            result = await NomenclatureTotalCalculate(result, serviceGroupGuid);

            return result;
        }

        /// <summary>
        /// Расчет номенклатуры книжной
        /// </summary>
        public async Task<NomenclatureOutDto> NomenclatureCalculateForm3(NomenclatureForm3InDto nomenclatureItem,
            string serviceGroupGuid)
        {
            var nomenclatureDto = new NomenclatureOutDto
            {
                nomenclature_guid = nomenclatureItem.nomenclature_guid,
                nomenclature_type_guid = nomenclatureItem.nomenclature_type_guid,
                order_guid = nomenclatureItem.order_guid,
                order_discount = nomenclatureItem.order_discount ?? 0,
                order_markup = nomenclatureItem.order_markup ?? 0,
                nomenclature_name = nomenclatureItem.nomenclature_name,
                cover_type_guid = nomenclatureItem.cover_type_guid,
                colorfulness1 = nomenclatureItem.colorfulness1 ?? 0,
                colorfulness2 = nomenclatureItem.colorfulness2 ?? 0,
                equipment_guid = nomenclatureItem.equipment_guid,
                complexity = nomenclatureItem.complexity ?? 0,
                priladki_count = nomenclatureItem.priladki_count ?? 0,
                contact_guid = nomenclatureItem.contact_guid,
                nomenclature_message = new List<string>()
            };

            var operationList = new List<NomenclatureOperationOutDto>();
            foreach (var no in nomenclatureItem.nomenclature_operation)
            {
                var o = new NomenclatureOperationOutDto
                {
                    operation_guid = no.operation_guid,
                    operation_count = no.operation_count,
                    operation_attribute = no.operation_attribute
                };
                operationList.Add(o);
            }

            var operationDto = await NomenclatureOperationDtoDataAdd(operationList, serviceGroupGuid);
            nomenclatureDto.nomenclature_operation = operationDto.ToList();

            var materialList = new List<NomenclatureMaterialOutDto>();
            foreach (var no in nomenclatureItem.nomenclature_material)
            {
                var o = new NomenclatureMaterialOutDto
                {
                    material_guid = no.material_guid,
                    material_count = no.material_count,
                    material_attribute = no.material_attribute
                };
                materialList.Add(o);
            }

            var materialDto = await NomenclatureMaterialDtoDataAdd(materialList, serviceGroupGuid);
            nomenclatureDto.nomenclature_material = materialDto.ToList();

            var result = await CalculateBooksNomenclatureField(nomenclatureDto, serviceGroupGuid);
            if (result.nomenclature_message.Count > 0) return result;

            result = await NomenclatureCalculateBook(result, serviceGroupGuid);
            if (result.nomenclature_message.Count > 0) return result;

            result = await NomenclatureOperationCalculate(result, serviceGroupGuid);
            if (result.nomenclature_message.Count > 0) return result;

            result = await NomenclatureMaterialCalculateForm3(result, serviceGroupGuid);
            if (result.nomenclature_message.Count > 0) return result;

            result = NomenclatureDiscountMarkupCalculate(result);
            if (result.nomenclature_message.Count > 0) return result;

            result = await NomenclatureTotalCalculate(result, serviceGroupGuid);

            return result;
        }

        /// <summary>
        /// Расчет номенклатуры листовой
        /// </summary>
        private async Task<NomenclatureOutDto> NomenclatureCalculateList(NomenclatureOutDto nomenclatureItem,
            string serviceGroupGuid)
        {
            var priladki_count = nomenclatureItem.priladki_count;

            var numberOfBands = nomenclatureItem.band_count;

            var nomenclatureTypeList = await _catalogDomain.CatalogGet("nomenclature_type",
                nomenclatureItem.nomenclature_type_guid, serviceGroupGuid, null);
            NomenclatureTypeDto nomenclatureType = nomenclatureTypeList.FirstOrDefault() as NomenclatureTypeDto;

            var equipmentPrintList =
                await _equipmentDomain.EquipmentGet(nomenclatureItem.equipment_guid, serviceGroupGuid);
            var equipmentPrint = equipmentPrintList.FirstOrDefault();

            nomenclatureItem.equipment_print_width = equipmentPrint?.max_width ?? 0;
            nomenclatureItem.equipment_print_height = equipmentPrint?.max_height ?? 0;

            var materialGuid = nomenclatureItem.nomenclature_material
                .Where(nm => nm.material_attribute.material_auto_added)
                .Select(nm => nm.material_guid).FirstOrDefault();

            var paperListList =
                await _catalogDomain.CatalogGet("paper_list", materialGuid, serviceGroupGuid, null);
            PaperListDto paperList = paperListList.FirstOrDefault() as PaperListDto;

            var a = nomenclatureItem.equipment_print_width;
            var b = nomenclatureItem.equipment_print_height;
            var c = nomenclatureItem.format_width;
            var d = nomenclatureItem.format_height;

            decimal colorfulness1 = nomenclatureItem.colorfulness1;
            decimal colorfulness2 = nomenclatureItem.colorfulness2;
            var productQuantity = nomenclatureItem.nomenclature_count;

            decimal resultCountProductPerList = 0;

            if (a == 0 || b == 0 || c == 0 || d == 0 || colorfulness1 == 0 || productQuantity == 0)
            {
                nomenclatureItem.nomenclature_message.Add("Не заполнены поля для расчета.");
                return nomenclatureItem;
            }

            int dol = 0;
            decimal a0 = 0;
            decimal b0 = 0;
            decimal c0 = a;
            decimal d0 = b;

            //Резка до печати
            if (paperList != null)
            {
                a0 = paperList.width ?? 0;
                b0 = paperList.height ?? 0;

                dol = CalculateProportionResult(a0, b0, c0, d0);
            }
            else
            {
                dol = 1;
            }

            nomenclatureItem.proportion_after_print = dol;

            //Расчет рабочего формата
            CalculateWorkFormat(dol, a0, b0, c0, d0, out a0, out b0);

            nomenclatureItem.work_format_width = (int) a0;
            nomenclatureItem.work_format_height = (int) b0;

            //Расчет размеров печатного поля
            decimal left_indent = equipmentPrint?.left_indent ?? 0;
            decimal right_indent = equipmentPrint?.right_indent ?? 0;
            decimal top_indent = equipmentPrint?.top_indent ?? 0;
            decimal bottom_indent = equipmentPrint?.bottom_indent ?? 0;
            a0 = a0 - left_indent - right_indent;
            b0 = b0 - top_indent - bottom_indent;

            nomenclatureItem.print_field_width = (int) a0;
            nomenclatureItem.print_field_height = (int) b0;

            if (c != 0 && d != 0 && a0 != 0 && b0 != 0)
            {
                // Кол-во изделий на одном листе
                if (a0 < b0)
                {
                    var bb = b0;
                    var aa = a0;
                    b0 = aa;
                    a0 = bb;
                }

                if (c < d)
                {
                    var cc = c;
                    var dd = d;
                    c = dd;
                    d = cc;
                }

                const int extraMargin = 4;
                var ce = c + extraMargin;
                var de = d + extraMargin;

                var format1 = Math.Floor(a0 / ce) * Math.Floor(b0 / de) +
                              Math.Floor((a0 - ce * Math.Floor(a0 / ce)) / de) * Math.Floor(b0 / ce);
                var format2 = Math.Floor(a0 / de) * Math.Floor(b0 / ce) +
                              Math.Floor((b0 - ce * Math.Floor(b0 / ce)) / de) * Math.Floor(a0 / ce);

                resultCountProductPerList = Math.Floor(format1) >= Math.Floor(format2)
                    ? Math.Floor(format1)
                    : Math.Floor(format2);
                if (resultCountProductPerList <= 0)
                    resultCountProductPerList = 1;
                if (nomenclatureItem.nomenclature_type_guid == "ed85bf7b-cbec-11e8-bf6a-00155dc210dc")
                    if (resultCountProductPerList == 10 || resultCountProductPerList == 12)
                        resultCountProductPerList = 8;
            }

            if (resultCountProductPerList <= 0)
                resultCountProductPerList = 1;

            nomenclatureItem.nomenclature_per_list_count = (int) resultCountProductPerList;

            //Листы
            var resultCountList = productQuantity / resultCountProductPerList;
            resultCountList = Convert.ToDecimal(Math.Ceiling(resultCountList) < 0
                ? "1"
                : Math.Ceiling(resultCountList).ToString(CultureInfo.CurrentCulture));

            //Листы - Вторая форма
            if (nomenclatureType.nomenclature_type_form_number == 2 && numberOfBands != 0)
                resultCountList = resultCountList / 2 * numberOfBands;

            nomenclatureItem.list_count = (int) resultCountList;

            //Листопрогон
            decimal equipmentPrintColorfulness = equipmentPrint.colorfulness ?? 0;
            var twoSidePrint = equipmentPrint.two_sided_print;
            if (equipmentPrintColorfulness != 0)
            {
                var res1 = Math.Ceiling(colorfulness1 / equipmentPrintColorfulness) *
                           resultCountList;
                var res2 = Math.Ceiling(colorfulness2 / equipmentPrintColorfulness) *
                           resultCountList;
                var x1 = res1 > res2 ? res1 : res2;
                var x2 = res1 + res2;
                var quantityProductionPrint = twoSidePrint ? (int) Math.Ceiling(x1) : Math.Ceiling(x2);

                nomenclatureItem.nomenclature_print_count = (int) quantityProductionPrint;
            }
            else
                nomenclatureItem.nomenclature_print_count = 0;

            //Кол-во форм
            var equipmentPrintIsDigit = equipmentPrint.digital;
            var countForm = equipmentPrintIsDigit ? 0 : colorfulness1 + colorfulness2;

            //Кол-во форм - Вторая форма
            if (nomenclatureType.nomenclature_type_form_number == 2 && numberOfBands != 0)
            {
                var objem = numberOfBands / resultCountProductPerList;
                countForm = equipmentPrintIsDigit
                    ? 0
                    : (int) Math.Ceiling(colorfulness1 + colorfulness2) * (int) Math.Ceiling(objem / 2);
            }

            nomenclatureItem.form_count = (int) decimal.Round(countForm, 0);

            //Приладки
            nomenclatureItem.priladki_count = equipmentPrintIsDigit
                ? priladki_count > 0 ? priladki_count : 1
                : (int) decimal.Round(countForm, 0);

            //Тетради
            nomenclatureItem.notebook_count = CalcNotebook(numberOfBands, resultCountProductPerList);

            return nomenclatureItem;
        }

        /// <summary>
        /// Расчет номенклатуры книжной
        /// </summary>
        private async Task<NomenclatureOutDto> NomenclatureCalculateBook(NomenclatureOutDto nomenclatureItem,
            string serviceGroupGuid)
        {
            var priladki_count = nomenclatureItem.priladki_count;

            var equipmentPrintList =
                await _equipmentDomain.EquipmentGet(nomenclatureItem.equipment_guid, serviceGroupGuid);
            var equipmentPrint = equipmentPrintList.FirstOrDefault();

            nomenclatureItem.equipment_print_width = equipmentPrint?.max_width ?? 0;
            nomenclatureItem.equipment_print_height = equipmentPrint?.max_height ?? 0;

            var materialGuid = nomenclatureItem.nomenclature_material
                .Where(nm =>
                    nm.material_attribute.material_auto_added &&
                    (nm.material_attribute.cover_material_contact || nm.material_attribute.cover_material_own))
                .Select(nm => nm.material_guid).FirstOrDefault();

            var paperListList =
                await _catalogDomain.CatalogGet("paper_list", materialGuid, serviceGroupGuid, null);
            PaperListDto paperList = paperListList.FirstOrDefault() as PaperListDto;

            var a = nomenclatureItem.equipment_print_width;
            var b = nomenclatureItem.equipment_print_height;
            var c = nomenclatureItem.format_width;
            var d = nomenclatureItem.format_height;

            decimal colorfulness1 = nomenclatureItem.colorfulness1;
            decimal colorfulness2 = nomenclatureItem.colorfulness2;
            var productQuantity = nomenclatureItem.nomenclature_count;
            decimal resultCountProductPerList = 0;

            if (a == 0 || b == 0 || c == 0 || d == 0 || colorfulness1 == 0 || productQuantity == 0)
                return new NomenclatureOutDto();

            int dol = 0;
            decimal a0 = 0;
            decimal b0 = 0;
            decimal c0 = a;
            decimal d0 = b;

            //Резка до печати
            if (paperList != null)
            {
                a0 = paperList.width ?? 0;
                b0 = paperList.height ?? 0;

                dol = CalculateProportionResult(a0, b0, c0, d0);
            }
            else
            {
                dol = 1;
            }

            nomenclatureItem.proportion_after_print = dol;

            //Расчет рабочего формата
            CalculateWorkFormat(dol, a0, b0, c0, d0, out a0, out b0);

            nomenclatureItem.work_format_width = (int) a0;
            nomenclatureItem.work_format_height = (int) b0;

            //Расчет размеров печатного поля
            decimal left_indent = equipmentPrint.left_indent ?? 0;
            decimal right_indent = equipmentPrint.right_indent ?? 0;
            decimal top_indent = equipmentPrint.top_indent ?? 0;
            decimal bottom_indent = equipmentPrint.bottom_indent ?? 0;
            a0 = a0 - left_indent - right_indent;
            b0 = b0 - top_indent - bottom_indent;

            nomenclatureItem.print_field_width = (int) a0;
            nomenclatureItem.print_field_height = (int) b0;

            if (c != 0 && d != 0 && a0 != 0 && b0 != 0)
            {
                // Кол-во изделий на одном листе
                if (a < b)
                {
                    var bb = b;
                    var aa = c;
                    b = aa;
                    a = bb;
                }

                if (c < d)
                {
                    var cc = c;
                    var dd = d;
                    c = dd;
                    d = cc;
                }

                const int extraMargin = 4;
                var ce = c + extraMargin;
                var de = d + extraMargin;

                var format1 = Math.Floor((decimal) (a / ce)) * Math.Floor((decimal) (b / de)) +
                              Math.Floor((a - ce * Math.Floor((decimal) (a / ce))) / de) *
                              Math.Floor((decimal) (b / ce));
                var format2 = Math.Floor((decimal) (a / de)) * Math.Floor((decimal) (b / ce)) +
                              Math.Floor((b - ce * Math.Floor((decimal) (b / ce))) / de) *
                              Math.Floor((decimal) (a / ce));

                resultCountProductPerList = Math.Floor(format1) >= Math.Floor(format2)
                    ? Math.Floor(format1)
                    : Math.Floor(format2);
                if (resultCountProductPerList <= 0)
                    resultCountProductPerList = 1;
            }

            if (resultCountProductPerList <= 0)
                resultCountProductPerList = 1;

            nomenclatureItem.nomenclature_per_list_count = (int) resultCountProductPerList;

            //Листы
            var resultCountList = productQuantity / resultCountProductPerList;
            resultCountList = Convert.ToDecimal(Math.Ceiling(resultCountList) < 0
                ? "1"
                : Math.Ceiling(resultCountList).ToString(CultureInfo.CurrentCulture));
            nomenclatureItem.list_count = (int) resultCountList;

            //Листопрогон
            decimal equipmentPrintColorfulness = equipmentPrint.colorfulness ?? 0;
            var twoSidePrint = equipmentPrint.two_sided_print;
            if (equipmentPrintColorfulness != 0)
            {
                var res1 = (Math.Ceiling((colorfulness1 / equipmentPrintColorfulness)) *
                            resultCountList);
                var res2 = (Math.Ceiling((colorfulness2 / equipmentPrintColorfulness)) *
                            resultCountList);
                var x1 = res1 > res2 ? res1 : res2;
                var x2 = res1 + res2;
                var quantityProductionPrint = twoSidePrint ? (int) Math.Ceiling(x1) : Math.Ceiling(x2);
                nomenclatureItem.nomenclature_print_count = (int) quantityProductionPrint;
            }
            else
                nomenclatureItem.nomenclature_print_count = 0;

            //Кол-во форм
            var equipmentPrintIsDigit = equipmentPrint.digital;
            var countForm = equipmentPrintIsDigit ? 0 : colorfulness1 + colorfulness2;
            nomenclatureItem.form_count = (int) decimal.Round(countForm, 0);

            //Приладки
            nomenclatureItem.priladki_count = equipmentPrintIsDigit
                ? priladki_count > 0 ? priladki_count : 1
                : (int) decimal.Round(countForm, 0);

            return nomenclatureItem;
        }

        /// <summary>
        /// Расчет формата обложки, формата сторонок, Отстав корешка, Расстав корешка
        /// </summary>
        private async Task<NomenclatureOutDto> CalculateBooksNomenclatureField(NomenclatureOutDto nomenclatureItem,
            string serviceGroupGuid)
        {
            //Номенклатуры
            //Блок тетрадный, блок листовой, вкладка
            var nomenclatureList = await NomenclatureGet(null, nomenclatureItem.order_guid, serviceGroupGuid);
            var nomenclatureListFiltered = nomenclatureList.Where(c =>
                c.nomenclature_type_guid == "ed85bf7b-cbec-11e8-bf6a-00155dc210dc"
                || c.nomenclature_type_guid == "ed85c8ed-cbec-11e8-bf6a-00155dc210dc"
                || c.nomenclature_type_guid == "ed85d2e4-cbec-11e8-bf6a-00155dc210dc").ToList();

            var nlOneElement = nomenclatureListFiltered.FirstOrDefault();

            double w = 0;
            var h = 0;

            //Бумага листовая
            var result = await _catalogDomain.CatalogGet("paper_list", null, serviceGroupGuid, null);
            var paperListList = result.Select(v => v as PaperListDto).ToList();

            //Отстав корешка
            var behindRoot = 0;
            var sumThikness = 0;
            //Типы обложки «Обложка мягкая», «Супер обложка»
            if (nomenclatureItem.cover_type_guid == "8e7e6ea0-d9d9-11e8-9d8d-00155dc210dc"
                || nomenclatureItem.cover_type_guid == "8157b5fe-d9d9-11e8-9d8d-00155dc210dc")
            {
                foreach (var res in nomenclatureList)
                {
                    var materialGuid = res.nomenclature_material
                        .Where(nm => nm.material_attribute.material_auto_added)
                        .Select(nm => nm.material_guid).FirstOrDefault();
                    sumThikness = paperListList.Where(c => c.paper_list_guid == materialGuid)
                                      .Select(c => c.thickness).FirstOrDefault() ?? 0;
                    decimal calc = ((decimal) res.band_count / 2) * sumThikness / 1000;
                    behindRoot += (int) Math.Ceiling(calc);
                }
            }
            else
            {
                double k2 = 0;
                foreach (var res in nomenclatureList)
                {
                    var materialGuid = res.nomenclature_material
                        .Where(nm => nm.material_attribute.material_auto_added)
                        .Select(nm => nm.material_guid).FirstOrDefault();
                    sumThikness = paperListList.Where(c => c.paper_list_guid == materialGuid)
                                      .Select(c => c.thickness).FirstOrDefault() ?? 0;
                    decimal calc = ((decimal) res.band_count / 2) * sumThikness / 1000;
                    var x2 = (int) Math.Ceiling(calc);

                    k2 = GetK(x2, k2);
                    if (x2 < 20)
                        behindRoot += (int) Math.Ceiling((decimal) (x2 + 2));
                    else
                        behindRoot += (int) Math.Ceiling(x2 * k2);
                }

            }

            //Формат обложки
            //Типы обложки «Обложка мягкая», «Супер обложка»
            if (nomenclatureItem.cover_type_guid == "8e7e6ea0-d9d9-11e8-9d8d-00155dc210dc"
                || nomenclatureItem.cover_type_guid == "8157b5fe-d9d9-11e8-9d8d-00155dc210dc")
            {
                if (nlOneElement != null)
                {
                    //Толщина бумаги мкм
                    w = nlOneElement.format_width * 2 + behindRoot;
                    h = nlOneElement.format_height;
                }
            }
            else
            {
                //Все остальные
                if (nlOneElement != null)
                {
                    var blockHight = nlOneElement.format_height;
                    if (blockHight < 40)
                        w = (nlOneElement.format_width + 6 + 8 + 15) * 2 + behindRoot;
                    else
                        w = (nlOneElement.format_width + 6 + 9 + 15) * 2 + behindRoot;

                    h = nlOneElement.format_height + 6 + 30;
                }
            }

            //Формат сторонок
            var sideAWidth = 0;
            var sideBHight = 0;
            if (nlOneElement != null)
            {
                sideAWidth = nlOneElement.format_width + 6;
                sideBHight = nlOneElement.format_height + 6;
            }

            //Расстав корешка
            var spineRoot = behindRoot < 40 ? 8 : 9;

            //Формат сторонок
            nomenclatureItem.storonki_a_width = sideAWidth;
            nomenclatureItem.storonki_b_height = sideBHight;

            //Форматы Корешка
            nomenclatureItem.format_width = Convert.ToInt32(w);
            nomenclatureItem.format_height = Convert.ToInt32(h);

            //Отстав корешка
            nomenclatureItem.otstav_koreshka = behindRoot;

            //Расстав корешка
            nomenclatureItem.rasstav_koreshka = spineRoot;

            return nomenclatureItem;
        }

        /// <summary>
        /// Расчет печати и добавление в список работ по номенклатуре
        /// </summary>
        private async Task<NomenclatureOutDto> NomenclatureOperationCalculate(NomenclatureOutDto nomenclature,
            string serviceGroupGuid)
        {
            //общий расчет
            if (nomenclature.nomenclature_operation != null)
            {
                foreach (var v in nomenclature.nomenclature_operation)
                {
                    v.operation_price_total = v.operation_price * v.operation_count;
                    v.operation_price_total_src = v.operation_price * v.operation_count;
                }
            }

            //подтягиваем данные по стоимости печати
            var colorfulnessPriceList =
                await EquipmentInfo.EquipmentColorfulnessEditionPriceGet(nomenclature.equipment_guid, serviceGroupGuid);
            var colorfulnessPrice = colorfulnessPriceList.FirstOrDefault(c =>
                nomenclature.list_count >= c.edition_min &&
                (nomenclature.list_count <= c.edition_max || c.edition_max == 0));

            //подтягиваем данные по стоимости приладки
            var adjustmentPriceList =
                await EquipmentInfo.EquipmentAdjustmentEditionPriceGet(nomenclature.equipment_guid, serviceGroupGuid);
            var adjustmentPrice = adjustmentPriceList
                .Where(c => nomenclature.priladki_count >= c.edition_min &&
                            (nomenclature.priladki_count <= c.edition_max || c.edition_max == 0)).Select(c => c.price)
                .FirstOrDefault();

            if (colorfulnessPrice == null)
            {
                nomenclature.nomenclature_message.Add("Не найдены цены на печать для выбранного оборудования.");
                return nomenclature;
            }

            if (adjustmentPrice == null)
            {
                nomenclature.nomenclature_message.Add("Не найдены цены на приладку для выбранного оборудования.");
                return nomenclature;
            }

            //расчитываем
            var colorfulnessPrice1 = colorfulnessPrice.GetColorPrice(nomenclature.colorfulness1);
            var colorfulnessPrice2 = colorfulnessPrice.GetColorPrice(nomenclature.colorfulness2);

            var priceTotal =
                decimal.Round(
                    (colorfulnessPrice1 + colorfulnessPrice2) * nomenclature.list_count +
                    adjustmentPrice * nomenclature.priladki_count ?? 0, 2);
            var price = decimal.Round(priceTotal / nomenclature.nomenclature_count, 2);

            //подтягиваем данные по работе
            var operationListObject = await _catalogDomain.CatalogGet("operation", null, serviceGroupGuid, null);
            var operationList = operationListObject.Select(v => v as OperationDto).ToList();
            var operation = operationList.FirstOrDefault(c => c.operation_name == "Печать");

            if (operation != null)
            {
                nomenclature.nomenclature_operation.RemoveAll(c => c.operation_attribute?.operation_auto_added == true);

                var result = new NomenclatureOperationOutDto
                {
                    nomenclature_guid = nomenclature.nomenclature_guid,
                    operation_guid = operation.operation_guid,
                    operation_name = operation.operation_name,
                    measure_guid = operation.measure_unit_guid,
                    measure_name = operation.measure_unit_name,
                    operation_price = price,
                    operation_count = nomenclature.nomenclature_count,
                    operation_price_total = priceTotal,
                    operation_price_total_src = priceTotal,
                    operation_attribute = new NomenclatureOperationAttribute {operation_auto_added = true}

                };
                nomenclature.nomenclature_operation.Add(result);
            }

            return nomenclature;
        }

        /// <summary>
        /// Расчет материала и добавление в список материалов по номенклатуре form 1
        /// </summary>
        private async Task<NomenclatureOutDto> NomenclatureMaterialCalculateForm1(NomenclatureOutDto nomenclature,
            string serviceGroupGuid)
        {
            //общий расчет
            if (nomenclature.nomenclature_material == null) return nomenclature;

            foreach (var v in nomenclature.nomenclature_material)
            {
                v.material_price_total = v.material_price * v.material_count;
                v.material_price_total_src = v.material_price * v.material_count;
            }


            //подтягиваем данные по материалу
            var materialListObject = await _catalogDomain.CatalogGet("paper_list", null, serviceGroupGuid, null);
            var materialList = materialListObject.Select(v => v as PaperListDto).ToList();
            var material = materialList
                .FirstOrDefault(c => c.paper_list_guid == nomenclature.nomenclature_material
                                         .Where(m => m.material_attribute.material_auto_added)
                                         .Select(m => m.material_guid)
                                         .FirstOrDefault());

            if (material == null) return nomenclature;

            //Коэффициент отходов бумаги при приладке
            var wasteRatioList =
                await EquipmentInfo.EquipmentAdjustmentGroupEditionGet(nomenclature.equipment_guid, serviceGroupGuid);
            var wasteRatio = wasteRatioList
                .Where(c => c.paper_list_type_guid == material.paper_list_type_guid &&
                            c.edition >= nomenclature.list_count).OrderBy(c => c.edition).FirstOrDefault();

            //Нормативы отходов бумаги
            var wasteListCountList =
                await EquipmentInfo.EquipmentAdjustmentGroupGet(nomenclature.equipment_guid, serviceGroupGuid);
            var wasteListCount =
                wasteListCountList.FirstOrDefault(c => c.paper_list_type_guid == material.paper_list_type_guid);

            if (wasteRatio == null || wasteListCount == null)
            {
                nomenclature.nomenclature_message.Add("Не найдены нормативы отходов для выбранного оборудования.");
                return nomenclature;
            }

            decimal quantity = nomenclature.list_count * (1 + wasteRatio.GetWasteRatio(nomenclature.complexity) / 100) +
                               nomenclature.priladki_count * wasteListCount.GetWasteListCount(nomenclature.complexity);

            quantity = decimal.Divide(quantity,
                nomenclature.proportion_after_print == 0 ? 1 : nomenclature.proportion_after_print);

            if (material.measure_unit_name == "лист")
                quantity = decimal.Ceiling(quantity);

            var priceTotal = decimal.Round(material.price ?? 0 * quantity, 2);
            var price = decimal.Round(priceTotal / quantity, 2);

            var newMaterial =
                nomenclature.nomenclature_material.FirstOrDefault(c => c.material_attribute.material_auto_added);

            newMaterial.nomenclature_guid = nomenclature.nomenclature_guid;
            newMaterial.material_type_guid = material.material_type_guid;
            newMaterial.material_guid = material.paper_list_guid;
            newMaterial.material_name = material.paper_list_name;
            newMaterial.measure_guid = material.measure_unit_guid;
            newMaterial.measure_name = material.measure_unit_name;
            newMaterial.material_price = price;
            newMaterial.material_count = quantity;
            newMaterial.material_price_total = priceTotal;
            newMaterial.material_price_total_src = priceTotal;

            nomenclature.nomenclature_material.RemoveAll(c => c.material_attribute.material_auto_added);

            nomenclature.nomenclature_material.Add(newMaterial);

            return nomenclature;
        }

        /// <summary>
        /// Расчет материала и добавление в список материалов по номенклатуре form 3
        /// </summary>
        private async Task<NomenclatureOutDto> NomenclatureMaterialCalculateForm3(NomenclatureOutDto nomenclature,
            string serviceGroupGuid)
        {
            //общий расчет
            if (nomenclature.nomenclature_material == null) return nomenclature;

            foreach (var v in nomenclature.nomenclature_material)
            {
                v.material_price_total = v.material_price * v.material_count;
                v.material_price_total_src = v.material_price * v.material_count;
            }

            //подтягиваем данные по материалу
            var materialListObject = await _catalogDomain.CatalogGet("paper_list", null, serviceGroupGuid, null);
            var materialList = materialListObject.Select(v => v as PaperListDto).ToList();
            var material1 = materialList
                .FirstOrDefault(c => c.paper_list_guid == nomenclature.nomenclature_material
                                         .Where(m => m.material_attribute.material_auto_added &&
                                                     (m.material_attribute.cover_material_contact ||
                                                      m.material_attribute.cover_material_own))
                                         .Select(m => m.material_guid)
                                         .FirstOrDefault());

            //подтягиваем данные по материалу
            var material2 = materialList
                .FirstOrDefault(c => c.paper_list_guid == nomenclature.nomenclature_material
                                         .Where(m => m.material_attribute.material_auto_added &&
                                                     (m.material_attribute.storonki_material_contact ||
                                                      m.material_attribute.storonki_material_own))
                                         .Select(m => m.material_guid)
                                         .FirstOrDefault());

            if (material1 == null || material2 == null) return nomenclature;

            //Коэффициент отходов бумаги при приладке
            var wasteRatioList =
                await EquipmentInfo.EquipmentAdjustmentGroupEditionGet(nomenclature.equipment_guid, serviceGroupGuid);
            var wasteRatio = wasteRatioList
                .Where(c => c.paper_list_type_guid == material1.paper_list_type_guid &&
                            c.edition >= nomenclature.list_count).OrderBy(c => c.edition).FirstOrDefault();

            //Нормативы отходов бумаги
            var wasteListCountList =
                await EquipmentInfo.EquipmentAdjustmentGroupGet(nomenclature.equipment_guid, serviceGroupGuid);
            var wasteListCount =
                wasteListCountList.FirstOrDefault(c => c.paper_list_type_guid == material1.paper_list_type_guid);

            if (wasteRatio == null || wasteListCount == null)
            {
                nomenclature.nomenclature_message.Add("Не найдены нормативы отходов для выбранного оборудования.");
                return nomenclature;
            }

            decimal quantity1 = nomenclature.list_count * (1 + wasteRatio.GetWasteRatio(nomenclature.complexity) / 100) +
                               nomenclature.priladki_count * wasteListCount.GetWasteListCount(nomenclature.complexity);

            quantity1 = decimal.Divide(quantity1,
                nomenclature.proportion_after_print == 0 ? 1 : nomenclature.proportion_after_print);

            if (material1.measure_unit_name == "лист") quantity1 = decimal.Ceiling(quantity1);

            var priceTotal1 = decimal.Round(material1.price ?? 0 * quantity1, 2);
            var price1 = decimal.Round(priceTotal1 / quantity1, 2);

            var quantity2 = nomenclature.list_count + nomenclature.priladki_count;
            var price2 = decimal.Round(material2.price ?? 0, 2);
            var priceTotal2 = decimal.Round(material2.price ?? 0 * quantity2, 2);


            var newMaterial1 = nomenclature.nomenclature_material.FirstOrDefault(c =>
                c.material_attribute.material_auto_added &&
                (c.material_attribute.cover_material_contact || c.material_attribute.cover_material_own));
            newMaterial1.nomenclature_guid = nomenclature.nomenclature_guid;
            newMaterial1.material_type_guid = material1.material_type_guid;
            newMaterial1.material_guid = material1.paper_list_guid;
            newMaterial1.material_name = material1.paper_list_name;
            newMaterial1.measure_guid = material1.measure_unit_guid;
            newMaterial1.measure_name = material1.measure_unit_name;
            newMaterial1.material_price = price1;
            newMaterial1.material_count = quantity1;
            newMaterial1.material_price_total = priceTotal1;
            newMaterial1.material_price_total_src = priceTotal1;

            var newMaterial2 = nomenclature.nomenclature_material.FirstOrDefault(c =>
                c.material_attribute.material_auto_added &&
                (c.material_attribute.storonki_material_contact || c.material_attribute.storonki_material_own));
            newMaterial2.nomenclature_guid = nomenclature.nomenclature_guid;
            newMaterial2.material_type_guid = material2.material_type_guid;
            newMaterial2.material_guid = material2.paper_list_guid;
            newMaterial2.material_name = material2.paper_list_name;
            newMaterial2.measure_guid = material2.measure_unit_guid;
            newMaterial2.measure_name = material2.measure_unit_name;
            newMaterial2.material_price = price2;
            newMaterial2.material_count = quantity2;
            newMaterial2.material_price_total = priceTotal2;
            newMaterial2.material_price_total_src = priceTotal2;


            nomenclature.nomenclature_material.RemoveAll(c => c.material_attribute.material_auto_added);

            nomenclature.nomenclature_material.Add(newMaterial1);
            nomenclature.nomenclature_material.Add(newMaterial2);

            return nomenclature;
        }

        /// <summary>
        /// Расчет общей суммы позиции
        /// </summary>
        private async Task<NomenclatureOutDto> NomenclatureTotalCalculate(NomenclatureOutDto nomenclature,
            string serviceGroupGuid)
        {
            var productsTotalSrc =
                nomenclature.nomenclature_operation?.Select(c => c.operation_price_total_src ?? 0).Sum();
            var materialsTotalSrc =
                nomenclature.nomenclature_material?.Select(c => c.material_price_total_src ?? 0).Sum();
            var priceTotalVatSrc = productsTotalSrc + materialsTotalSrc;

            var productsTotal = nomenclature.nomenclature_operation?.Select(c => c.operation_price_total ?? 0).Sum();
            var materialsTotal = nomenclature.nomenclature_material?.Select(c => c.material_price_total ?? 0).Sum();
            var priceTotalVat = productsTotal + materialsTotal;

            var account = await _contactDomain.ContactBankAccountGet(null, nomenclature.contact_guid, serviceGroupGuid);
            var order_vat_value = account.Select(a => a.vat ?? 0).FirstOrDefault();

            var vatAmount = priceTotalVat - priceTotalVat / (1 + decimal.Multiply(order_vat_value, (decimal) 0.01));
            var priceTotalSrc = priceTotalVatSrc - vatAmount;
            var priceTotal = priceTotalVat - vatAmount;

            nomenclature.price_total_vat = decimal.Round(priceTotalVat ?? 0, 2);
            nomenclature.price_total_vat_src = decimal.Round(priceTotalVatSrc ?? 0, 2);

            nomenclature.price_total = decimal.Round(priceTotal ?? 0, 2);
            nomenclature.price_total_src = decimal.Round(priceTotalSrc ?? 0, 2);

            nomenclature.vat_value = order_vat_value;
            nomenclature.vat_amount = decimal.Round(vatAmount ?? 0, 2);

            return nomenclature;
        }

        /// <summary>
        /// Расчет скидки/наценки
        /// </summary>
        private NomenclatureOutDto NomenclatureDiscountMarkupCalculate(NomenclatureOutDto nomenclature)
        {
            if (nomenclature.nomenclature_operation != null)
            {
                foreach (var v in nomenclature.nomenclature_operation)
                {
                    if (nomenclature.order_discount > 0)
                    {
                        v.operation_price_total = decimal.Round((v.operation_price_total_src ?? 0) -
                                                                (v.operation_price_total_src ?? 0) *
                                                                (decimal) (nomenclature.order_discount * 0.01), 2);
                    }

                    if (nomenclature.order_markup > 0)
                    {
                        v.operation_price_total = decimal.Round((v.operation_price_total_src ?? 0) +
                                                                (v.operation_price_total_src ?? 0) *
                                                                (decimal) (nomenclature.order_markup * 0.01), 2);
                    }
                }
            }

            if (nomenclature.nomenclature_material != null)
            {
                foreach (var v in nomenclature.nomenclature_material)
                {
                    if (nomenclature.order_discount > 0)
                    {
                        v.material_price_total = decimal.Round((v.material_price_total_src ?? 0) -
                                                               (v.material_price_total_src ?? 0) *
                                                               (decimal) (nomenclature.order_discount * 0.01), 2);
                    }

                    if (nomenclature.order_markup > 0)
                    {
                        v.material_price_total = decimal.Round((v.material_price_total_src ?? 0) +
                                                               (v.material_price_total_src ?? 0) *
                                                               (decimal) (nomenclature.order_markup * 0.01), 2);
                    }
                }
            }

            return nomenclature;
        }

        /// <summary>
        /// Расчет скидки/наценки по заказу
        /// </summary>
        public OrderFullDto OrderDiscountMarkupCalculate(OrderFullDto order)
        {
            var nomenclatureList = new List<NomenclatureOutDto>();
            foreach (var v in order.nomenclature)
            {
                v.order_discount = order.order_discount;
                v.order_markup = order.order_markup;
                nomenclatureList.Add(NomenclatureDiscountMarkupCalculate(v));
            }

            order.nomenclature = nomenclatureList;

            order.order_price_total = order.nomenclature.Sum(c => c.price_total);
            order.order_price_total_vat = order.nomenclature.Sum(c => c.price_total_vat);
            order.order_vat_amount = order.nomenclature.Sum(c => c.vat_amount);

            return order;
        }
    }
}