CREATE  procedure MGS_SP_GET_DEBITNOTE
AS BEGIN
select 
--GeneralData
a."DocEntry" AS "DocEntry",
a."DocNum" AS "Ref",
'FacturaComercial' AS "Type",
a."DocDate" AS "Date",
a."DocCur" AS "Currency",
a."DocRate" AS "ExchangeRate",
'false' TaxIncluded,
a."U_ACS_NCF" AS "NCF",
a."DocDueDate" AS "NCFExpirationDate",
--PublicAdministration
'01' AS "TipoIngreso",
a."U_ACS_FORMA_PAGO" AS "FormaPago",
(select count(*) from INV1 T0 WHERE t0."DocEntry" = a."DocEntry") AS "LinesPerPrint",
--Supplier
'130873' AS "sSupplierID",
'VoxelCaribe SRL' AS "sCompany",
'130873011' AS "sCIF",
'CFilomena Gómez de Cova' AS "sAddress",
'DOM' AS "sCountry",
'Santo Domingo' AS "sCity",
'1' AS "sPC",
'DN' AS "sProvince",
'alertasdgii@voxelgroup.net' AS "sEmail",
--Client
c."LicTradNum" AS "cCIF",
c."CardName" AS "cCompany",
c."E_Mail" AS "cEmail",
(select max("Street") from CRD1 z WHERE z."CardCode" = a."CardCode" and "AdresType" = 'B'  )  AS "cAddress",
'Santo Domingo' AS "cCity",
'08012' AS "cPC", 
'Distrito Nacional' AS "Province",
'DOM' AS "Country",
--InvoiceRef
--(select x."DocNum" from  OINV x WHERE x."DocEntry" = B."BaseEntry"  ) AS "InvoiceRef",
A."U_MGS_FE_REF" AS "InvoiceRef",
A."U_MGS_FE_RefNCF" AS "InvoiceNCF",
TO_VARCHAR(A."U_MGS_FE_RefFecha",'yyyy-MM-dd') AS "InvoiceRedDate",
--(select x."U_ACS_NCF" from  OINV x WHERE x."DocEntry" = B."BaseEntry"  ) AS "InvoiceNCF",
--(select x."DocDate" from  OINV x WHERE x."DocEntry" = B."BaseEntry"  ) AS "InvoiceRedDate",
'3' AS "CodigoModificacion", -- DOM.CodigoModificacion,
'0' AS "IndicadorNotaCredito", -- DOM.IndicadorNotaCredito,
--ProducList
b."ItemCode" AS "SupplierSKU",
b."Dscription" AS "Item",
b."Quantity" AS "Qty",
'Unidades' AS "MU",
'0' AS "CU", --,
b."Price" AS "UP",
CASE WHEN a."DocCur" = 'DOP' THEN b."LineTotal" else b."TotalFrgn" end AS "Total",
CASE WHEN a."DocCur" = 'DOP' THEN b."LineTotal" else b."TotalFrgn" end AS "NetAmount", --
'Purchase' AS "SysLineType",
--Taxes
(select Z."Name" from OSTA Z WHERE z."Code" =b."TaxCode") AS "txType",
a."DocRate" AS "Rate"  ,
CASE WHEN a."DocCur" = 'DOP' THEN (b."LineTotal" -  b."VatSum") else (b."TotalFrgn" -  b."VatSumFrgn") end AS "txBase", 
CASE WHEN a."DocCur" = 'DOP' THEN b."VatSum" else b."VatSumFrgn" end AS "txAmount",
--TotalSummary

CASE WHEN a."DocCur" = 'DOP' THEN a."BaseAmnt" else a."BaseAmntSC" end AS "tSubTotal"  ,
CASE WHEN a."DocCur" = 'DOP' THEN (a."DocTotal"- a."BaseAmnt" ) else (a."DocTotalFC"- a."BaseAmntSC" )end AS "tTax"  ,
CASE WHEN a."DocCur" = 'DOP' THEN a."DocTotal" else a."DocTotalFC" end AS "tTotal"  


 from OINV A
join INV1 B ON (A."DocEntry" = b."DocEntry")
join OCRD c on (a."CardCode" = c."CardCode")
where a."DocSubType" ='DN' AND a."U_MGS_FE_Estado" ='DP' AND a."U_ACS_NCF" IS NOT NULL AND a."DocEntry" >300;
END