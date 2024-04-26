CREATE procedure MGS_SP_GET_CREDITNOTE
AS BEGIN
select 
--GeneralData
--a."DocNum" "Ref",
a."DocEntry" AS "DocEntry",
a."U_ACS_NCF" AS "Ref",
'FacturaAbono' "Type",
a."DocDate" "Date",
a."DocCur" "Currency",
a."DocRate" "ExchangeRate",
'false' "TaxIncluded",
a."U_ACS_NCF" "NCF",
a."DocDueDate" "NCFExpirationDate",
--PublicAdministration
'01' "TipoIngreso",
a."U_ACS_FORMA_PAGO" "FormaPago",
(select count(*) from "INV1" T0 WHERE t0."DocEntry" = a."DocEntry") "LinesPerPrint",
--Supplier
'130873' "sSupplierID",
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
--InvoiceRef
(select x."DocNum" from  "OINV" x WHERE x."DocEntry" = B."BaseEntry"  ) "InvoiceRef",
(select x."U_ACS_NCF" from  "OINV" x WHERE x."DocEntry" = B."BaseEntry"  ) "InvoiceNCF",
(select TO_VARCHAR(x."DocDate",'yyyy-MM-dd') from  "OINV" x WHERE x."DocEntry" = B."BaseEntry"  ) "InvoiceRedDate",
'3' "DOM.CodigoModificacion",
'0' "DOM.IndicadorNotaCredito",
--ProducList
b."ItemCode" "SupplierSKU",
b."Dscription" "Item",
b."Quantity" "Qty",
'Unidades' "MU",
'0' "CU", --?,
b."Price" "UP",
CASE WHEN a."DocCur" = 'DOP' THEN b."LineTotal" else b."TotalFrgn" end "Total",
CASE WHEN a."DocCur" = 'DOP' THEN b."LineTotal" else b."TotalFrgn" end "NetAmount", --?
'Purchase'  "SysLineType",
--Taxes
(select Z."Name" from "OSTA" Z WHERE z."Code" =b."TaxCode") "txType",
a."DocRate" "Rate"  ,
CASE WHEN a."DocCur" = 'DOP' THEN (b."LineTotal" -  b."VatSum") else (b."TotalFrgn" -  b."VatSumFrgn") end "txBase", 
CASE WHEN a."DocCur" = 'DOP' THEN b."VatSum" else b."VatSumFrgn" end "txAmount",
--TotalSummary

CASE WHEN a."DocCur" = 'DOP' THEN a."BaseAmnt" else a."BaseAmntSC" end "tSubTotal"  ,
CASE WHEN a."DocCur" = 'DOP' THEN (a."DocTotal"- a."BaseAmnt" ) else (a."DocTotalFC"- a."BaseAmntSC" )end "tTax"  ,
CASE WHEN a."DocCur" = 'DOP' THEN a."DocTotal" else a."DocTotalFC" end "tTotal"  


 from "ORIN" A
join "RIN1" B ON (A."DocEntry" = b."DocEntry")
join "OCRD" c on (a."CardCode" = c."CardCode")
WHERE A."DocEntry">147 AND a."U_MGS_FE_Estado" ='DP' AND a."U_ACS_NCF" IS NOT NULL;
END