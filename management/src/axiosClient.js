import axios from 'axios';
import router from './router';

const axiosClient = axios.create({
  baseURL: 'https://localhost:7024/api/', 
});

axiosClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers['Authorization'] = `Bearer ${token}`;
  }
  return config;
}, (error) => {
  return Promise.reject(error);
});

axiosClient.interceptors.response.use(undefined, function (error) {
  if (error.response.status === 401 || error.response.status === 403) {
    localStorage.removeItem('token');
    alert('Phiên đăng nhập hết hạn. Vui lòng đăng nhập lại.');
    router.push('/login');
  }
  return Promise.reject(error);
});


export default axiosClient;

//import axios from 'axios';

// Tạo instance của axios
// const api = axios.create({
//   baseURL: 'https://api.example.com',
// });

// Thiết lập interceptor để xử lý lỗi hết hạn token
// api.interceptors.response.use(
//   response => response,
//   async error => {
//     if (error.response.status === 401) {
//       try {
//         // Gửi request làm mới token
//         const refreshToken = localStorage.getItem('refreshToken');
//         const response = await axios.post('https://api.example.com/auth/refresh-token', { token: refreshToken });

//         // Lưu lại token mới
//         localStorage.setItem('accessToken', response.data.accessToken);
        
//         // Gửi lại request gốc với token mới
//         error.config.headers['Authorization'] = 'Bearer ' + response.data.accessToken;
//         return api.request(error.config);
//       } catch (refreshError) {
//         // Xử lý nếu làm mới token thất bại (ví dụ: điều hướng tới trang đăng nhập)
//         console.log('Token hết hạn và làm mới token thất bại', refreshError);
//         window.location.href = '/login';
//         return Promise.reject(refreshError);
//       }
//     }
//     return Promise.reject(error);
//   }
// );

// export default api;

