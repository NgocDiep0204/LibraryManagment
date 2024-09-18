import axiosClient from '../axiosClient';

export async function fetchAuthor({ commit, state }) {
  if (state.authors && state.authors.length > 0) return;
    try {
      const response = await axiosClient.get('/Author');
      if (response.data) {
        commit('setAuthors', response.data);
      } else {
        throw new Error('Failed to fetch authors data');
      }
    } catch (error) {
      console.error('Error fetching authors:', error);
      throw error;
    }
  }
  
  export async function fetchCategory({ commit, state }) {
    if (state.categories && state.categories.length > 0) return;
    try {
      const response = await axiosClient.get('Categories/GetAllCategories');
      if (response.data && typeof response.data === 'object' && response.data.$values && Array.isArray(response.data.$values)) {
        commit('setCategories', response.data.$values);
      } else {
        throw new Error('Response data is not in the expected format or "$values" array is missing.');
      }
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw error;
    }
  }
  export async function fetchPosition({ commit, state }) {
    if (state.positions && state.positions.length > 0) return;
    try {
      const response = await axiosClient.get('Postitions/GetAllPostion');
      if (response.data && typeof response.data === 'object' && response.data.$values && Array.isArray(response.data.$values)) {
        commit('setPositions', response.data.$values);
      } else {
        throw new Error('Response data is not in the expected format or "$values" array is missing.');
      }
    } catch (error) {
      console.error('Error fetching positions:', error);
      throw error;
    }
  }
  export async function fetchPublisher({ commit, state }) {
    if (state.publishers && state.publishers.length > 0) return;
    try {
      const response = await axiosClient.get('Publicators/GetAllPublicators');
      if (response.data && typeof response.data === 'object' && response.data.$values && Array.isArray(response.data.$values)) {
        commit('setPublishers', response.data.$values);
      } else {
        throw new Error('Response data is not in the expected format or "$values" array is missing.');
      }
    } catch (error) {
      console.error('Error fetching publishers:', error);
      throw error;
    }
  }