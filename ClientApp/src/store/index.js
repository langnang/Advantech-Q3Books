import Vue from 'vue'
import Vuex from 'vuex'
import booklist from "./../../booklist.json";
import moment from 'moment';
import api from './../api'
Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    booklist: booklist,
    book_filter: {
      page: 1,
      page_size: 100,
      id: "",
      name: "",
      price: 0,
      price_min: 0,
      price_max: 100,
      discount_min: 0,
      discount_max: 100,
      group_price: 0,
      group_price_min: 0,
      group_price_max: 100,
      type: "",
      inventory: "",
      two_grades: "",
      three_grades: "",
      author: "",
      translator: "",
      isbn: "",
      publisher: "",
    },
    config: {},
    user: {},
    job_number: "",
    shopping: [],
    users: [],
    books: [],
  },
  mutations: {
    setConfig(state, config) {
      state.config = config;
    },
    setBooks(state, books) {
      state.books = books;
    },
    setUser(state, user) {
      state.user = user;
    },
    setUsers(state, users) {
      state.users = users;
    },
    deleteBook(state, index) {
      state.shopping.splice(index, 1);
    },
    addBook(state, book) {
      state.shopping.push(book);
    },
    setShopping(state, shopping) {
      state.shopping = shopping;
    },
    setBookFilterPage(state, page) {
      state.book_filter.page = page;
    },
    setBookFilterPageSize(state, size) {
      state.book_filter.page_size = size;
    }
  },
  actions: {
    login({ commit }, job_number) {
      api.user.login(job_number)
        .then(res => {
          if (res.data.status == 200) {
            commit("setUser", res.data.data);
            if (!sessionStorage.getItem("user")) {
              sessionStorage.setItem("user", job_number);
              location.reload();
            }
          } else {
            alert("登录失败，请重试！！");
          }
        })
    },
    getConfig({ commit }) {
      this._vm.$axios.get("/api/config")
        .then(function (res) {
          const config = res.data.data.reduce(function (total, value) {
            total[value.name] = value.value;
            return total;
          }, {})
          commit("setConfig", config)
        })
    },
    getBooks({ commit }) {
      api.book.all().then(function (res) {
        commit("setBooks", res.data.data)
      })
    },
    getUsers({ commit }) {
      api.user.all().then(function (res) {
        commit("setUsers", res.data.data)
      })
    },
    getShopping({ commit, getters }, job_number) {
      api.book.list(job_number).then(res => {
        if (res.data.status == 200) {
          commit("setShopping", res.data.data.map(v => {
            if (v.custom) {
              return {
                ...v,
                _book: JSON.parse(v.annotation)
              }
            } else {
              return {
                ...v,
                _book: getters.book(v.book)
              }
            }

          }));
        }
      })
    },
  },
  getters: {
    discount(state) {
      return parseFloat(state.config.discount || 0.88);
    },
    expire(state) {
      return moment(state.config.end_date || '2020-09-05') < new Date();
    },
    maxPrice(state) {
      return parseFloat(state.config.max_price || 100);
    },
    booklist(state) {
      return state.booklist;
    },
    sumOfPrice(state) {
      return state.shopping.reduce(function (prev, book) {
        if (book.custom) {
          return prev + book._book.price * 0.69;
        } else {
          return prev + book._book.group_price;
        }
      }, 0)
    },
    sumOfDiscount(state, getters) {
      return (getters.sumOfPrice * 0.88).toFixed(3);
    },
    book() {
      return function (id) {
        if (id) {
          return booklist.filter(book => book.id == id)[0];
        } else {
          return {};
        }
      }
    },
    allBookCart(state) {
      return state.books.map(function (value) {
        const result = {
          ...value,
          _user: state.users.filter(user => user.job_number == value.user)[0],
          _book: state.booklist.filter(book => book.id == value.book)[0]
        };
        return result;
      })
    },
    countCustom(state) {
      return state.shopping.reduce((prev, v) => {
        if (v.custom) {
          prev++;
        }
        return prev;
      }, 0)
    },
    booklist_pages(state, getters) {
      const maxPage = Math.ceil(getters.booklist_computed.length / state.book_filter.page_size);
      if (maxPage <= state.book_filter.page) {
        state.book_filter.page = maxPage;
      }
      if (state.book_filter.page == 0) {
        state.book_filter.page = 1;
      }
      return getters.booklist_computed.slice(
        (state.book_filter.page - 1) * state.book_filter.page_size,
        state.book_filter.page * state.book_filter.page_size
      );
    },
    booklist_computed(state) {
      return state.booklist.filter(function (val) {
        return (
          val.id.toString().indexOf(state.book_filter.id) > -1 &&
          val.name.toString().indexOf(state.book_filter.name) > -1 &&
          val.group_price >= state.book_filter.group_price_min &&
          val.group_price <= state.book_filter.group_price_max &&
          val.two_grades.toString().indexOf(state.book_filter.two_grades) > -1 &&
          val.author.toString().indexOf(state.book_filter.author) > -1 &&
          val.publisher.toString().indexOf(state.book_filter.publisher) > -1 &&
          val.inventory > 0
        );
      });
    },
  },
  modules: {
  }
})
