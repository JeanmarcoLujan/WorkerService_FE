CREATE  procedure MGS_SP_GET_INVOICE
AS
BEGIN

SELECT * FROM 
(
select 
--GeneralData
a."DocEntry" AS "DocEntry",
--a."DocSubType" AS "DocSubType",
a."U_ACS_NCF" "Ref",
'FacturaComercial' "Type",
to_date(a."DocDate") "Date",
a."DocCur" "Currency",
a."DocRate" "ExchangeRate",
case when (select count(*) from "INV1" z where z."DocEntry"= a."DocEntry" and z."TaxCode"  like '%ITB%' )>0 then 'true' else 'false' end "TaxIncluded",
a."U_ACS_NCF" "NCF",
(select "U_ACS_Valido_Hasta" from "@ACS_NCF_DETAILS" where "Code" ='FACTURAS' and "U_ACS_TIPO_NCF" =A."U_ACS_TIPO_NCF" AND "U_ACS_NCF_ACTIVO" = 'Y') "NCFExpirationDate",
--PublicAdministration
'01' "TipoIngreso",
case when a."GroupNum" = '8' then '1' else '2' end "TipoPago",
(select count(*) from "INV1" T0 WHERE t0."DocEntry" = a."DocEntry") "LinesPerPrint",
--Supplier
'130873011' "sSupplierID",
'VoxelCaribe SRL' "sCompany",
'130873011' "sCIF",
'C/Filomena Gómez de Cova' "sAddress",
'DOM' "sCountry",
'Santo Domingo' "sCity",
'1' "sPC",
'DN' "sProvince",
'alertasdgii@voxelgroup.net' "sEmail",
--Client
c."LicTradNum" "cCIF",
c."CardName" "cCompany",
c."E_Mail" "cEmail",
(select max("Street") from "CRD1" z WHERE z."CardCode" = a."CardCode" and "AdresType" = 'B'  )  "cAddress",
'Santo Domingo' "cCity",
'08012' "cPC", 
'Distrito Nacional' "Province",
'DOM' "Country",
--ProducList
b."ItemCode" "SupplierSKU",
b."Dscription" "Item",
b."Quantity" "Qty",
'Unidades' "MU",
'0' "CU", --?,
b."Price" "UP",
CASE WHEN a."DocCur" = 'DOP' THEN b."LineTotal" else b."TotalFrgn" end "Total",
CASE WHEN a."DocCur" = 'DOP' THEN b."LineTotal" else b."TotalFrgn" end "NetAmount", --?
case when b."U_ACS_TIPO_MONTO" ='B' then 'Purchase' else 'GenericServices' end  "SysLineType",
--Taxes
e."U_ACS_Clase_Impuesto" "txType",
b."VatPrcnt" "Rate"  ,
CASE WHEN a."DocCur" = 'DOP' THEN (b."LineTotal" -  b."VatSum") else (b."TotalFrgn" -  b."VatSumFrgn") end "txBase", 
CASE WHEN a."DocCur" = 'DOP' THEN b."VatSum" else b."VatSumFrgn" end "txAmount",

--DueDates
to_date(a."DocDueDate") "DueDateCredit",
CASE WHEN a."DocCur" = 'DOP' THEN a."DocTotal" else a."DocTotalFC" end "AmountCredit",
SUBSTRING (a."U_ACS_FORMA_PAGO",2,2) "PaymentID",

--TotalSummary

CASE WHEN a."DocCur" = 'DOP' THEN a."BaseAmnt" else a."BaseAmntSC" end "tSubTotal"  ,
CASE WHEN a."DocCur" = 'DOP' THEN (a."DocTotal"- a."BaseAmnt" ) else (a."DocTotalFC"- a."BaseAmntSC" )end "tTax"  ,
CASE WHEN a."DocCur" = 'DOP' THEN a."DocTotal" else a."DocTotalFC" end "tTotal" ,
CASE WHEN a."DocCur" = 'DOP' THEN a."VatSum" else a."VatSumFC" end "ImpuestoGrl",
CASE WHEN a."DocCur" = 'DOP' THEN (a."DocTotal" - a."VatSum") else (a."DocTotalFC" - a."VatSumFC") end "SubTotalGrl",
CASE WHEN a."DocCur" = 'DOP' THEN a."DocTotal" else a."DocTotalFC" end "TotalGrl",
d."WTCode" "CodigoRetencion",
d."WTAmnt" "CantidadRetenida"



 from "OINV" A
join "INV1" B ON (A."DocEntry" = b."DocEntry")
join "OCRD" c on (a."CardCode" = c."CardCode")
left join "INV5" d on (d."AbsEntry" = a."DocEntry")
join "@ACS_CLASES_IMP" E ON (B."TaxCode" = e."Code")
where a."U_MGS_FE_Estado" ='DP' and a."DocSubType"= '--' AND a."U_ACS_NCF" IS NOT NULL 
AND A."DocDate">='20240321'
) AS MGS
WHERE "NCFExpirationDate" IS NOT NULL;
END