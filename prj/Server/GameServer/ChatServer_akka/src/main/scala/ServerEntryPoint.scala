import Service.GameServer
import akka.actor.ActorSystem
import akka.http.scaladsl.ConnectionContext
import akka.http.scaladsl.Http
import akka.stream.Materializer

import java.io.InputStream
import scala.io.StdIn
import java.security.{KeyStore, SecureRandom}
import javax.net.ssl.{KeyManagerFactory, SSLContext, TrustManagerFactory}

object ServerEntryPoint {
  def main(args: Array[String]): Unit = {

    implicit val system = ActorSystem()
    implicit val materializer = Materializer
    implicit val executionContext = system.dispatcher

    val key: KeyStore = KeyStore.getInstance("PKCS12")
    val keyStoreFile: InputStream = getClass.getClassLoader.getResourceAsStream("myKeystore.p12")
    val password = "MY_PASSWORD".toCharArray
    key.load(keyStoreFile, password)

    val keyManagerFactory = KeyManagerFactory.getInstance("SunX509")
    keyManagerFactory.init(key, password)

    val trustManagerFactory = TrustManagerFactory.getInstance("SunX509")
    trustManagerFactory.init(key)

    val sslContext: SSLContext = SSLContext.getInstance("TLS")
    sslContext.init(keyManagerFactory.getKeyManagers, trustManagerFactory.getTrustManagers, new SecureRandom)

    val httpsConnectionContext = ConnectionContext.httpsServer(sslContext)


    val gameService = new GameServer()

    //val bindingFutureWithoutSecurity = Http().newServerAt("127.0.0.1",8000).bindFlow(gameService.FinalRoute)
    val bindingFutureWithSecurity = Http().newServerAt("127.0.0.1",8002).enableHttps(httpsConnectionContext).bindFlow(gameService.FinalRoute)

    println(s"Server is progressing...\nPress RETURN to stop...")
    StdIn.readLine()
    bindingFutureWithSecurity
      .flatMap(_.unbind())
      .onComplete(_ => system.terminate())
  }
}
