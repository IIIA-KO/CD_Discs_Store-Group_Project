import './App.css';
import Home from './pages/Home/Home'
import Disks from './pages/Disks/Disks'
import Films from './pages/Films/Films'
import Music from './pages/Music/Music'

import Cart from './pages/Cart/Card';
import Profile from './pages/Profile/Profile';
import {BrowserRouter,Routes,Route,Link} from 'react-router-dom'
import Header from './Header/Header';
import Footer from './Footer/Footer';
import FilmDetails from './pages/FilmDetails/FilmDetails'
import MusicDetails from './pages/MusicDetails/MusicDetails'
import AdminPanel from './pages/AdminPanel/AdminPanel';
import AdminDisks from './pages/AdminDisks/AdminDisks';
import AdminDisksAdd from './pages/AdminDisks/AdminDisksAdd';
import AdminDisksDelete from './pages/AdminDisks/AdminDisksDelete';
import AdminDisksEdit from './pages/AdminDisks/AdminDisksEdit';


function App() {
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
                <Route exact path="/music/:id" element={<MusicDetails/>} />
                <Route path='/cart' element={<Cart />} />
                <Route path='/profile' element={<Profile />} />
                <Route path='/adminpanel' element={<AdminPanel />} />
                <Route path='/adminpanel/disks' element={<AdminDisks />} />
                <Route path='/adminpanel/disks/add' element={<AdminDisksAdd />} />
                <Route path='/adminpanel/disks/delete/:id' element={<AdminDisksDelete />} />
                <Route path='/adminpanel/disks/edit/:id' element={<AdminDisksEdit />} />

            </Routes>
            </main>
            <Footer />
        </>
    );
}

export default App;