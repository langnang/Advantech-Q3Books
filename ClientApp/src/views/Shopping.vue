<template>
	<div>
		<el-table :data="shopping">
			<el-table-column label="书号" prop="id">
				<template slot-scope="scope">{{scope.row._book.id+""}}</template>
			</el-table-column>
			<el-table-column label="书单名称" prop="_book.name"></el-table-column>
			<el-table-column label="定价" prop="_book.price" width="70"></el-table-column>
			<el-table-column label="折扣价" prop="_book.price" width="70" v-if="false">
				<template slot-scope="scope">{{(scope.row._book.price * 0.88).toFixed(3)}}</template>
			</el-table-column>
			<el-table-column label="书单类别" prop="_book.type"></el-table-column>
			<el-table-column label="作者" prop="_book.author"></el-table-column>
			<el-table-column label="译者" prop="_book.translator"></el-table-column>
			<el-table-column label="ISBN分类" prop="_book.isbn"></el-table-column>
			<el-table-column label="出版社" prop="_book.version"></el-table-column>
			<el-table-column label="操作" width="100">
				<template slot-scope="scope">
					<!-- <el-button size="mini" type="primary" @click="updateBook(scope.$index, scope.row)">编辑</el-button> -->
					<el-popconfirm
						icon="el-icon-info"
						iconColor="red"
						title="确定要删除该图书吗？"
						@onConfirm="confirmDeleteBook(scope.$index, scope.row)"
					>
						<el-button size="mini" type="danger" slot="reference" :disabled="expire">删除</el-button>
					</el-popconfirm>
				</template>
			</el-table-column>
		</el-table>
		<el-divider></el-divider>
		<h1>
			<el-tag
				type="info"
				effect="dark"
				style="font-size:32px;height:52px;line-height:50px;"
			>总价：{{ sumOfPrice }}</el-tag>
			<el-tag
				type="warning"
				effect="dark"
				style="margin-left:20px;font-size:32px;height:52px;line-height:50px;"
				v-if="false"
			>折扣价：{{ sumOfDiscount }}</el-tag>
		</h1>
	</div>
</template>

<script>
	import { mapState, mapGetters } from "vuex";
	export default {
		name: "app",
		components: {},
		data() {
			return {
				dialogVisible: true,
				bookDialogVisible: false,
				input: "",
				form: {
					name: "",
					price: 0,
					pc_link: "",
				},
				tableData: [],
			};
		},
		created() {},
		computed: {
			...mapState(["shopping"]),
			...mapGetters(["expire", "discount", "sumOfPrice"]),
		},
		methods: {
			confirmDeleteBook: function (index, row) {
				const _this = this;
				this.$axios({
					method: "delete",
					url: "api/book/delete",
					data: {
						user: sessionStorage.getItem("user"),
						book: row._book.id,
					},
				}).then(function (res) {
					if (res.data.status == 200) {
						_this.$store.commit("deleteBook", index);
						_this.$message({
							message: "书籍删除成功！！！",
							type: "success",
						});
						_this.tableData.splice(index, 1);
					} else {
						_this.$message({
							message: "书籍删除失败！！！",
							type: "error",
						});
					}
				});
			},
		},
	};
</script>
