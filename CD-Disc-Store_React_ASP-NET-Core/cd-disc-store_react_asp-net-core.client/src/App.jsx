import './App.css';
import Home from './pages/Home/Home'
import Disks from './pages/Disks/Disks'
import Films from './pages/Films/Films'
import Music from './pages/Music/Music'

import Cart from './pages/Cart/Card';
import Profile from './pages/Profile/Profile';
import {BrowserRouter,Routes,Route,Link} from 'react-router-dom'

import Header from './Header/Header';


function App() {
    
    return (
        <>
         <Header/>
         <Routes>
            <Route path='/' element={<Home/>}/>
            <Route path='/disks' element={<Disks/>}/>
            <Route path='/films' element={<Films/>}/>
            <Route path='/music' element={<Music/>}/>
            <Route path='/cart' element={<Cart/>}/>
            <Route path='/profile' element={<Profile/>}/>
         </Routes>
        </>
    );
}
export default App;