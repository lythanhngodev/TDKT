<%@ Page Title="Test" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="TDKT.Test" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="container">
		<div id="getCBCC">
			<table class="table bordered">
				<tr>
					<th colspan="2" >Lấy dữ liệu từ PHP</th>
				</tr>
				<tr>
					<td style="width:120px">Tên miền</td>
					<td><input type="text" id="tenMienGui"class="form-control" value="http://localhost/NTServices/Services/getCBCC.php" style="width:100%"></td>
				</tr>
				<tr>
					<td></td>
					<td><button id="guiDuLieuCBCC" class="btn btn-primary">Lấy dữ liệu</button></td>
				</tr>
                <tr>
                    <td>Chuỗi kết quả</td>
                    <td id="resultJson1">
                    </td>
                </tr>
			</table>
            <hr />
            <table class="table bordered">
				<tr>
					<th colspan="2" style="width:120px">Gửi dữ liệu đến PHP</th>
				</tr>
				<tr>
					<td style="width:120px">Tên miền</td>
					<td><input type="text" id="tenMienNhan"class="form-control" value="http://localhost/NTServices/Services/setCBCC.php" style="width:100%"></td>
				</tr>
				<tr>
					<td></td>
					<td><button id="nhanDuLieuCBCC" class="btn btn-primary">Gửi dữ liệu</button></td>
				</tr>
                <tr>
                    <td>Chuỗi kết quả</td>
                    <td id="resultJson2">
                    </td>
                </tr>
			</table>
		</div>
	</div>
<script type="text/javascript">
	$(document).ready(function(){
        $(document).on('click', '#guiDuLieuCBCC', () => {
            var result = getAjax('string', 'Services/WebService.asmx/getCBCCfromPHP', { data: $('#tenMienGui').val() });
            /*if (result.split('_')[0] == '1') {
                alert('Thành công');
            } else {
                alert('Không thành công!');
            }*/
            $('#resultJson1').html(result);
            return false;
        });
        $(document).on('click', '#nhanDuLieuCBCC', () => {
            var result = getAjax('string', 'Services/WebService.asmx/setCBCCtoPHP', { url: $('#tenMienNhan').val(), postData: '' });
            /*if (result.split('_')[0] == '1') {
                alert('Thành công');
            } else {
                alert('Không thành công!');
            }*/
            $('#resultJson2').html(result);
            return false;
        });
	});
    function getAjax(kieuTraVe, duongDanAjax, duLieuGui) {
        var result = null;
        $.ajax({
            type: "POST",
            url: duongDanAjax,
            data: JSON.stringify(duLieuGui),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            timeout: 1000000, // thời gian chờ
            beforeSend: function () {

            },
            success: function (response) {
                result = JSON.parse(response.d);
            },
            error: function (response) {
                result = null;
            },
            complete: function () {

            }
        });
        return result;
    }
</script>
</asp:Content>
