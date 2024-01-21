import './footer.css';
const Footer = () => {
    return (
        <>
            <footer>
                <div className="authors">
                    <ul className='team'>
                        <li>Team Lead</li>
                        <li>Oleksander Vlasiuk</li>
                    </ul>
                    <ul className='team'>
                        <li>Architect</li>
                        <li>Illia Kotvitskiy</li>
                    </ul>
                    <ul className='team'>
                        <li>Project manager</li>
                        <li>Sophia Yachmeniova</li>
                    </ul>
                    <ul className='team'>
                        <li>Designers/Frontenders</li>
                        <li>Daryna Arkhypchuk</li>
                        <li>Diana Gorbachevska</li>
                        <li>David Darabian</li>
                    </ul>
                    <ul className='team'>
                        <li>Testers</li>
                        <li>Vitaliy Korzh</li>
                        <li>Oleksiy Novakivskiy</li>
                    </ul>
                    <ul className='team'>
                        <li>Programmers</li>
                        <li>Vasyl Nedashkivskiy</li>
                        <li>Ostap Sushytskiy</li>
                    </ul>
                </div>
                <div className='copyright'>
                    <span>@P01 2023-2024</span>
                </div>

            </footer>
        </>
    )
}

export default Footer