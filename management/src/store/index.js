import { createStore } from 'vuex';
import axiosClient from '../axiosClient';

// State
const state = {
  total: 0,
  authors: [],
  categories: [],
  positions: [],
  publishers: [],
  rentials: [],
  originalRentials: [],
  countbookbymonth: [], 
  countrentsbymonth: [],
  user: null,
  token: localStorage.getItem('token') || null,  // Lấy token từ localStorage
};

// Mutations
const mutations = {
  saveOriginalRentials(state) {
    state.originalRentials = JSON.parse(JSON.stringify(state.rentials));
  },
 
  setCountBookByMonth(state, countbookbymonth) {
    state.countbookbymonth = countbookbymonth;
  },
  setCountRentsByMonth(state, countrentsbymonth) {
    state.countrentsbymonth = countrentsbymonth;
  },
  setRentials(state, rentials) {
    state.rentials = rentials;
  },
  setAuthors(state, authors) {
    state.authors = authors;
  },
  setCategories(state, categories) {
    state.categories = categories;
  },
  setPositions(state, positions) {
    state.positions = positions;
  },
  setPublishers(state, publishers) {
    state.publishers = publishers;
  },
  setUser(state, user) {
    state.user = user;
  },
  setToken(state, token) {
    state.token = token;
  },
  clearAuthData(state) {
    state.user = null;
    state.token = null;
  },
  logout(state) {
    state.user = null; 
  },
  updateProduct(state, updatedRental) {
    const index = state.rentials.findIndex(rental => rental.rentId === updatedRental.rentId);
    if (index !== -1) {
      const existingRental = state.rentials[index];
      // Kết hợp thông tin cũ vào đối tượng mới cập nhật
      state.rentials[index] = {
        ...existingRental,
        ...updatedRental,
        studentIdNavigation: existingRental.studentIdNavigation
      };
    }
  },
  settoltals(state, total) {
    state.total = total;
  }
};

// Actions
const actions = {
  logout({ commit }) {
    commit('logout');
},
  async fetchUserInfo({ commit }) {
    try {
      const response = await axiosClient.get('Authenlication/GetUserProfile');
      commit('setUser', response.data);
    } catch (error) {
      console.error('Error fetching user info:', error);
    }
  },

  async fetchCountRentsByMonth({ commit }) {
    try {
      const response = await axiosClient.get('Rents/GetCountRentsByMonth');
      commit('setCountRentsByMonth', response.data.$values);
    } catch (error) {
      console.error('Error fetching count rents by month:', error);
    }
  },

  async fetchCountBookByMonth({ commit }) {
    try {
      const response = await axiosClient.get('Rents/GetCountBooksByMonth');
      commit('setCountBookByMonth', response.data.$values);
    } catch (error) {
      console.error('Error fetching count book by month:', error);
    }
  },

  async fetchRential({ commit, state }, pageNumber ) {
    try {
      const response = await axiosClient.get(`Rents/GetAllRentials/${pageNumber}`);
      commit('setRentials', response.data.pagedRentials.$values);
      commit('saveOriginalRentials');
      commit('settoltals', response.data.totalPages);
    } catch (error) {
      console.error('Error fetching rentals:', error);
    }
  },

  async fetchAuthor({ commit, state }) {
    try {
      const response = await axiosClient.get('Athors/GetAll');
      commit('setAuthors', response.data.$values);
    } catch (error) {
      console.error('Error fetching authors:', error);
    }
  },

  async fetchCategory({ commit, state }) {
    try {
      const response = await axiosClient.get('Categories/GetAllCategories');
      commit('setCategories', response.data.$values);
    } catch (error) {
      console.error('Error fetching categories:', error);
    }
  },

  async fetchPosition({ commit, state }) {
    try {
      const response = await axiosClient.get('Postitions/GetAllPostion');
      commit('setPositions', response.data.$values);
    } catch (error) {
      console.error('Error fetching positions:', error);
    }
  },

  async fetchPublisher({ commit, state }) {
     try {
      const response = await axiosClient.get('Publicators/GetAllPublicators');
      commit('setPublishers', response.data.$values);
    } catch (error) {
      console.error('Error fetching publishers:', error);
    }
  },

  async login({ commit }, credentials) {
    try {
      const response = await axiosClient.post('Authenlication/Login', credentials);
      const token = response.data.token;
      localStorage.setItem('token', token);
      commit('setToken', token);
      commit('setUser', response.data.user);
    } catch (error) {
      console.error('Error logging in:', error);
      throw error;
    }
  },

  logout({ commit }) {
    localStorage.removeItem('token');
    commit('clearAuthData');
  },
};

const getters = {
  //getUser: (state) => state.user,
  isAuthenticated: (state) => !!state.token,
  getRentials: state => state.rentials,
};

// Create Store
const store = createStore({
  state,
  actions,
  mutations,
  getters,
});

export default store;
