import './App.css';
import Home from './pages/Home/Home'
import Disks from './pages/Disks/Disks'
import Films from './pages/Films/Films'
import Music from './pages/Music/Music'

import Cart from './pages/Cart/Cart';
import Profile from './pages/Profile/Profile';
import {BrowserRouter,Routes,Route,Link} from 'react-router-dom'
import Header from './Header/Header';
import Footer from './Footer/Footer';
import FilmDetails from './pages/FilmDetails/FilmDetails'


import { useState } from 'react';

function App() {
    const [cartItems, setCartItems] = useState([]);
    return (
        <>
            <Header />
            <main>
            <Routes>
                <Route path='/' element={<Home />} />
                <Route path='/disks' element={<Disks />} />
                <Route path='/films' element={<Films />} />
                <Route exact path="/films/:id" element={<FilmDetails/>} />
                <Route path='/music' element={<Music />} />
                <Route path='/cart' element={<Cart items={cartItems} />} />
                <Route path='/profile' element={<Profile />} />
            </Routes>
            </main>
            <Footer />
        </>
    );
}

export default App;