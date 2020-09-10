<template>
	<div>
		<p style="color:red;">友情提醒：请在9月10日（本周四）17:00前完成书箱（仅限当当自营）选购，过期系统将自动锁定，无法编辑，谢谢！</p>
		<el-table :data="shopping" height="calc(100vh - 240px)">
			<el-table-column label="书号" prop="_book.id" width="90">
				<template slot-scope="scope">
					<el-link
						type="primary"
						target="_blank"
						:href="'http://product.dangdang.com/'+scope.row._book.id+'.html'"
					>{{scope.row._book.id}}</el-link>
				</template>
			</el-table-column>
			<el-table-column label="书名" prop="_book.name">
				<template slot-scope="scope">
					<span v-if="scope.row.custom">
						<el-tag size="medium" type="success" effect="dark" v-if="scope.row.book<1000000000">自营</el-tag>
						<el-tag size="medium" type="danger" effect="dark" v-else>非自营</el-tag>
						{{scope.row._book.name}}
					</span>
					<span v-else>{{scope.row._book.name}}</span>
				</template>
			</el-table-column>
			<el-table-column label="定价" prop="_book.price" width="70" v-if="false"></el-table-column>
			<el-table-column label="团购价" prop="_book.group_price" width="70">
				<template slot-scope="scope">
					<span v-if="scope.row.custom">{{(scope.row._book.price*0.69).toFixed(2)}}</span>
					<span v-else>{{scope.row._book.group_price}}</span>
				</template>
			</el-table-column>
			<el-table-column label="折扣价" prop="_book.price" width="70" v-if="false">
				<template slot-scope="scope">{{(scope.row._book.price * 0.88).toFixed(2)}}</template>
			</el-table-column>
			<el-table-column label="分类" prop="_book.two_grades" width="120"></el-table-column>
			<el-table-column label="作者" prop="_book.author"></el-table-column>
			<!-- <el-table-column label="译者" prop="_book.translator"></el-table-column> -->
			<!-- <el-table-column label="ISBN分类" prop="_book.isbn"></el-table-column> -->
			<el-table-column label="出版社" prop="_book.publisher" width="150"></el-table-column>
			<el-table-column label="操作" width="90">
				<template slot-scope="scope">
					<!-- <el-button size="mini" type="primary" @click="updateBook(scope.$index, scope.row)">编辑</el-button> -->
					<el-button
						size="mini"
						type="primary"
						slot="reference"
						v-if="false"
						:disabled="expire"
						style="display:inline-block;"
						@click="showCustomBook(scope.row)"
					>编辑</el-button>
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
		<el-button-group>
			<el-button id="addBookBtn" type="primary" :disabled="expire" @click="showCustomBook">新增自选书</el-button>
			<el-button id="sumPriceBtn" type="info">总价：{{ sumOfPrice.toFixed(2) }}</el-button>
			<el-link
				id="onlineBtn"
				class="el-button el-button--danger is-plain"
				target="_blank"
				:underline="false"
				href="http://book.dangdang.com/"
				v-if="false"
			>
				<span>在线选书</span>
			</el-link>
			<el-link
				id="onlineBtn"
				class="el-button el-button--danger is-plain"
				target="_blank"
				:underline="false"
				href="http://book.dangdang.com/"
				style="padding:0;border:1px;height:61px;"
			>
				<img src="./../assets/dangdang.png" />
			</el-link>
		</el-button-group>
		<h1 v-if="false">
			<el-button
				type="primary"
				style="font-size:29px;"
				:disabled="expire||countCustom>=2"
				@click="showCustomBook"
			>新增自选书</el-button>
			<el-tag
				type="info"
				effect="dark"
				style="font-size:32px;height:55px;line-height:55px;"
			>总价：{{ sumOfPrice.toFixed(2) }}</el-tag>
			<el-tag
				type="warning"
				effect="dark"
				style="font-size:32px;height:55px;line-height:55px;"
				v-if="false"
			>折扣价：{{ sumOfDiscount.toFixed(2) }}</el-tag>
			<el-link
				type="primary"
				target="_blank"
				:underline="false"
				href="http://book.dangdang.com/"
				style
			>
				<el-tag type="danger" effect="plain" style="font-size:32px;height:55px;line-height:55px;">在线选书</el-tag>
			</el-link>
			<el-link
				type="primary"
				target="_blank"
				:underline="false"
				href="http://book.dangdang.com/"
				style
			>
				<img src="<%= BASE_URL %>dangdang.png" />
			</el-link>
		</h1>
		<el-dialog title="新增自选书" :visible.sync="customBookDialogVisible" @closed="resetBookForm">
			<el-form :model="book_form" ref="book_form" :rules="rules" label-width="80px">
				<el-form-item label="链接" prop="link_id">
					<el-input v-model="book_form.link_id" @input="getBookInfo">
						<template slot="prepend">http://product.dangdang.com/</template>
						<template slot="append">.html</template>
					</el-input>
				</el-form-item>
				<el-form-item label="书号" prop="id">
					<el-input v-model="book_form.id" :readonly="true" :disabled="true"></el-input>
				</el-form-item>
				<el-form-item label="书名" prop="name">
					<el-input v-model="book_form.name" :readonly="true" :disabled="true"></el-input>
				</el-form-item>
				<el-form-item label="作者" prop="author">
					<el-input v-model="book_form.author" :readonly="true" :disabled="true"></el-input>
				</el-form-item>
				<el-form-item label="出版社" prop="publisher">
					<el-input v-model="book_form.publisher" :readonly="true" :disabled="true"></el-input>
				</el-form-item>
				<el-form-item label="定价" prop="price">
					<el-input v-model="book_form.price" :readonly="true" :disabled="true"></el-input>
				</el-form-item>
				<el-button type="primary" @click="confirmAddBook" :disabled="submitBookForm">确认</el-button>
			</el-form>
		</el-dialog>
	</div>
</template>

<script>
	import { mapState, mapGetters } from "vuex";
	export default {
		name: "app",
		components: {},
		data() {
			return {
				customBookDialogVisible: false,
				input: "",
				book_form: {
					link_id: "",
					id: "",
					name: "",
					author: "",
					publisher: "",
					price: "",
				},
				submitBookForm: false,
				rules: {
					link_id: [
						{
							required: true,
							message: "请输入书单号（纯数字），例：29002852",
							triggle: "blur",
						},
					],
				},
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
			...mapGetters([
				"expire",
				"discount",
				"sumOfPrice",
				"maxPrice",
				"countCustom",
			]),
		},
		methods: {
			showCustomBook: function (row) {
				if (row._book) {
					this.book_form = row._book;
				} else {
					this.book_form = {
						link_id: "",
						id: "",
						name: "",
						author: "",
						publisher: "",
						price: "",
					};
					this.submitBookForm = true;
				}
				this.customBookDialogVisible = true;
			},
			confirmDeleteBook: function (index, row) {
				const _this = this;
				this.$axios({
					method: "delete",
					url: "api/book/delete",
					data: {
						user: sessionStorage.getItem("user"),
						book: row._book.id,
						id: row.id,
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
			confirmAddBook: function () {
				let _this = this;
				this.$refs.book_form.validate((valid) => {
					if (valid) {
						if (!_this.book_form.ddzy) {
							alert(
								"添加失败：所选书籍并非当当自营，请重新选择书籍！！！"
							);
						} else if (
							_this.sumOfPrice + _this.book_form.price * 0.69 >
							_this.maxPrice
						) {
							alert("添加失败：总价超出限制，请重新选择书籍！！！");
						} else {
							_this
								.$axios({
									method: "put",
									url: "api/book/add",
									data: {
										user: sessionStorage.getItem("user"),
										book: _this.book_form.id,
										custom: true,
										annotation: JSON.stringify({
											..._this.book_form,
											group_price:
												_this.book_form.price * 0.69,
										}),
									},
								})
								.then(function (res) {
									if (res.data.status == 200) {
										_this.$store.commit("addBook", {
											...res.data.data,
											_book: JSON.parse(
												res.data.data.annotation
											),
										});
										_this.$message({
											message: "添加购物车成功！！！",
											type: "success",
										});
										_this.customBookDialogVisible = false;
									} else {
										_this.$message({
											message: "添加购物车失败！！！",
											type: "error",
										});
									}
								});
						}
					} else {
						return false;
					}
				});
			},
			resetBookForm() {
				this.book_form = {
					link_id: "",
					id: "",
					name: "",
					author: "",
					publisher: "",
					price: "",
				};
				this.$refs["book_form"].resetFields();
			},
			getBookInfo(value) {
				const _this = this;
				this.$axios.get(`/api/book/${value}`).then(function (res) {
					if (
						res.data.data &&
						res.data.data.id == _this.book_form.link_id
					) {
						if (res.data.status == 200) {
							_this.book_form = { ...res.data.data, link_id: value };
							_this.submitBookForm = false;
						} else {
							_this.book_form.id = "";
							_this.book_form.name = "";
							_this.book_form.author = "";
							_this.book_form.publisher = "";
							_this.book_form.price = "";
							_this.submitBookForm = true;
						}
					} else {
						_this.book_form.id = "";
						_this.book_form.name = "";
						_this.book_form.author = "";
						_this.book_form.publisher = "";
						_this.book_form.price = "";
						_this.submitBookForm = true;
					}
				});
			},
		},
	};
</script>

<style soped>
	#addBookBtn,
	#sumPriceBtn,
	#onlineBtn {
		font-size: 35px;
		border-radius: 10px;
	}
	#sumPriceBtn,
	#onlineBtn {
		margin-left: 30px;
	}
	#onlineBtn {
		border-color: #fbc4c4;
	}
	#sumPriceBtn:hover,
	#sumPriceBtn:visited,
	#sumPriceBtn:focus,
	#sumPriceBtn:active {
		color: #fff;
		background-color: #909399;
		border-color: #909399;
	}
</style>